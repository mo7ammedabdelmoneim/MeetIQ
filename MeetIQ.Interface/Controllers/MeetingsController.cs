using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;
using System.Security.Claims;
using MeetIQ.Infrastructure.Services;
using MeetIQ.Interface.ViewModels;
using MeetIQ.Infrastructure.Presistence;

namespace MeetIQ.Interface.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly JitsiTokenService _jitsiTokenService;
        private readonly IConfiguration _config;

        public MeetingsController(
            ApplicationDbContext db,
            JitsiTokenService jitsiTokenService,
            IConfiguration config)
        {
            _db = db;
            _jitsiTokenService = jitsiTokenService;
            _config = config;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        private string CurrentUserName => User.Identity?.Name ?? "User";

        // ─── INDEX ───────────────────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var meetings = await _db.Meetings
                .Include(m => m.Host)
                .Include(m => m.Participants).ThenInclude(p => p.User)
                .Where(m => m.HostId == CurrentUserId
                         || m.Participants.Any(p => p.UserId == CurrentUserId))
                .OrderByDescending(m => m.ScheduledAt)
                .ToListAsync();

            return View(meetings);
        }

        // ─── DETAILS ─────────────────────────────────────────────────────────────
        public async Task<IActionResult> Details(Guid id)
        {
            var meeting = await _db.Meetings
                .Include(m => m.Host)
                .Include(m => m.Participants).ThenInclude(p => p.User)
                .Include(m => m.Notes)
                .Include(m => m.Tasks)
                .Include(m => m.Summary)
                .Include(m => m.Transcript)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (meeting == null) return NotFound();

            bool canAccess = meeting.HostId == CurrentUserId
                || meeting.Participants.Any(p => p.UserId == CurrentUserId);

            if (!canAccess) return Forbid();

            return View(meeting);
        }

        // ─── CREATE ──────────────────────────────────────────────────────────────
        public IActionResult Create() => View(new CreateMeetingViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMeetingViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var meetingId = Guid.NewGuid();

            var meeting = new Meeting
            {
                Id = meetingId,
                Title = vm.Title,
                JitsiRoomId = GenerateRoomId(vm.Title),
                ScheduledAt = vm.ScheduledAt.ToUniversalTime(),
                Status = MeetingStatus.Scheduled,
                HostId = CurrentUserId,
            };

            _db.Meetings.Add(meeting);
            await _db.SaveChangesAsync(); // Save meeting first so FK exists

            // Now add host as participant with valid FK
            var hostParticipant = new MeetingParticipant
            {
                Id = Guid.NewGuid(),
                MeetingId = meetingId,
                UserId = CurrentUserId,
                JoinedAt = DateTime.UtcNow,
            };
            _db.MeetingParticipants.Add(hostParticipant);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = meetingId });
        }

        // ─── JOIN ────────────────────────────────────────────────────────────────
        public async Task<IActionResult> Join(Guid id)
        {
            // Load meeting WITHOUT tracking participants — we manage them separately
            var meeting = await _db.Meetings
                .Include(m => m.Host)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (meeting == null) return NotFound();

            if (meeting.Status == MeetingStatus.Ended ||
                meeting.Status == MeetingStatus.Cancelled)
            {
                TempData["Error"] = "This meeting has already ended.";
                return RedirectToAction(nameof(Details), new { id = id.ToString() });
            }

            bool isHost = meeting.HostId == CurrentUserId;

            // ── Handle participant registration independently ──────────────────
            // Check if this user already has a participant record
            var existingParticipant = await _db.MeetingParticipants
                .FirstOrDefaultAsync(p => p.MeetingId == id && p.UserId == CurrentUserId);

            if (existingParticipant == null)
            {
                // New participant — insert fresh row
                var newParticipant = new MeetingParticipant
                {
                    Id = Guid.NewGuid(),
                    MeetingId = id,
                    UserId = CurrentUserId,
                    JoinedAt = DateTime.UtcNow,
                    LeftAt = null,
                };
                _db.MeetingParticipants.Add(newParticipant);
            }
            else
            {
                // Existing participant rejoining — update join time
                existingParticipant.JoinedAt = DateTime.UtcNow;
                existingParticipant.LeftAt = null;
                _db.MeetingParticipants.Update(existingParticipant);
            }

            // ── Update meeting status if host is joining for the first time ────
            if (isHost && meeting.StartedAt == null)
            {
                await _db.Meetings
                    .Where(m => m.Id == id)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(m => m.StartedAt, DateTime.UtcNow)
                        .SetProperty(m => m.Status, MeetingStatus.InProgress));
            }

            await _db.SaveChangesAsync();

            // ── Build Jitsi token if configured ───────────────────────────────
            string? token = null;
            if (_config.GetValue<bool>("Jitsi:UseToken"))
            {
                token = _jitsiTokenService.GenerateToken(
                    meeting.JitsiRoomId,
                    CurrentUserId,
                    CurrentUserName,
                    isHost);
            }

            var vm = new JitsiRoomViewModel
            {
                Meeting = meeting,
                JwtToken = token,
                IsHost = isHost,
                JitsiDomain = _config["Jitsi:Domain"] ?? "meet.jit.si",
                DisplayName = CurrentUserName,
                AvatarUrl = $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(CurrentUserName)}&background=1332e1&color=fff",
            };

            return View(vm);
        }

        // ─── END (host only) ─────────────────────────────────────────────────────
        // FIX: Use ExecuteUpdateAsync to avoid concurrency issues.
        //      RedirectToAction uses explicit string key to avoid C# tuple ambiguity.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> End(Guid id)
        {
            // Verify caller is the host
            var meeting = await _db.Meetings
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (meeting == null)
                return NotFound();

            if (meeting.HostId != CurrentUserId)
            {
                TempData["Error"] = "Only the host can end this meeting.";
                return RedirectToAction(nameof(Details), new { id = id.ToString() });
            }

            if (meeting.Status != MeetingStatus.Ended)
            {
                var endedAt = DateTime.UtcNow;

                // Update meeting directly via ExecuteUpdate — no tracking conflict
                await _db.Meetings
                    .Where(m => m.Id == id)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(m => m.EndedAt, endedAt)
                        .SetProperty(m => m.Status, MeetingStatus.Ended));

                // Mark all participants as left
                await _db.MeetingParticipants
                    .Where(p => p.MeetingId == id && p.LeftAt == null)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(p => p.LeftAt, endedAt));
            }

            TempData["Success"] = "Meeting ended. You can now upload a transcript and generate a summary.";

            // Explicit route value — avoids the redirect-to-End bug
            return RedirectToAction(nameof(Details), new { id = id.ToString() });
        }

        // ─── LEAVE (participant) ──────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Leave(Guid id)
        {
            await _db.MeetingParticipants
                .Where(p => p.MeetingId == id && p.UserId == CurrentUserId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.LeftAt, DateTime.UtcNow));

            return RedirectToAction(nameof(Index));
        }

        // ─── DELETE (host only) ───────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var meeting = await _db.Meetings
                .FirstOrDefaultAsync(m => m.Id == id && m.HostId == CurrentUserId);

            if (meeting == null)
            {
                TempData["Error"] = "Meeting not found or you are not the host.";
                return RedirectToAction(nameof(Index));
            }

            _db.Meetings.Remove(meeting);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Meeting deleted.";
            return RedirectToAction(nameof(Index));
        }

        // ─── HELPERS ─────────────────────────────────────────────────────────────
        private static string GenerateRoomId(string title)
        {
            var slug = new string(
                title.ToLower().Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray()
            ).Trim('-');

            return $"meetiq-{slug}-{Guid.NewGuid().ToString("N")[..8]}";
        }
    }
}