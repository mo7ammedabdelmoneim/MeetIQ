using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeetIQ.Application.Features.Meetings.Commands.CancelMeetingCommand;
using MeetIQ.Application.Features.Meetings.Commands.CreateMeetingCommand;
using MeetIQ.Application.Features.Meetings.Commands.EndMeetingCommand;
using MeetIQ.Application.Features.Meetings.Commands.JoinMeetingCommand;
using MeetIQ.Application.Features.Meetings.Commands.LeaveMeetingCommand;
using MeetIQ.Application.Features.Meetings.Commands.StartMeetingCommand;
using MeetIQ.Application.Features.Meetings.Queries.GetMeetingByIdQuery;
using MeetIQ.Application.Features.Meetings.Queries.GetMeetingsQuery;
using MeetIQ.Application.Interfaces.Services;
using MeetIQ.Domain.Enums;
using MeetIQ.Web.ViewModels.Meetings;
using MeetIQ.Application.Features.Meetings.Commands.InviteUserCommand;
using MeetIQ.Application.Features.Meetings.Commands.RespondToInvitationCommand;
using MeetIQ.Application.Features.Meetings.Commands.RevokeInvitationCommand;
using MeetIQ.Application.Features.Meetings.Queries.SearchUsersToInviteQuery;
using MeetIQ.Application.Features.Meetings.Queries.GetUserPendingInvitationsQuery;
using MeetIQ.Application.Features.Transcripts.Commands.TranscribeMeetingCommand;
using MeetIQ.Application.Features.Meetings.Commands.AnalyzeMeetingCommand;
using MeetIQ.Application.Features.Meetings.Commands.ConfirmAnalysisCommand;
using MeetIQ.Application.Features.Meetings.Commands.PreviewAnalysisCommand;
using MeetIQ.Application.Features.Meetings.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MeetIQ.Web.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
        private readonly IMediator mediator;
        private readonly IJitsiTokenService jitsiTokenService;
        private readonly IRecordingService recordingService;

        public MeetingsController(IMediator mediator, IJitsiTokenService jitsiTokenService, IRecordingService recordingService)
        {
            this.mediator = mediator;
            this.jitsiTokenService = jitsiTokenService;
            this.recordingService = recordingService;
        }

        private string CurrentUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        private string CurrentUserName => User.FindFirst("FullName")?.Value ?? "User";
        private string CurrentUserEmail => User.FindFirst(ClaimTypes.Email)?.Value ?? "";

        
        [HttpGet]
        public async Task<IActionResult> Index(
            MeetingStatus? status,
            string? search,
            DateTime? from,
            DateTime? to,
            int page = 1)
        {
            ViewData["Title"] = "Meetings";

            var result = await mediator.Send(new GetMeetingsQuery
            {
                UserId = CurrentUserId,
                Status = status,
                Search = search,
                From = from,
                To = to,
                Page = page
            });

            ViewBag.CurrentStatus = status;
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentFrom = from?.ToString("yyyy-MM-dd");
            ViewBag.CurrentTo = to?.ToString("yyyy-MM-dd");

            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "New Meeting";
            return View(new CreateMeetingViewModel());
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMeetingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "New Meeting";
                return View(model);
            }

            var id = await mediator.Send(new CreateMeetingCommand
            {
                Title = model.Title,
                ScheduledAt = model.ScheduledAt.ToUniversalTime(),
                HostId = CurrentUserId
            });

            TempData["Success"] = "Meeting created successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewData["Title"] = "Meeting Details";

            var meeting = await mediator.Send(new GetMeetingByIdQuery { MeetingId = id });
            if (meeting == null) return NotFound();

            ViewBag.IsHost = meeting.HostId == CurrentUserId;
            ViewBag.CurrentUid = CurrentUserId;

            return View(meeting);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(Guid meetingId)
        {
            await mediator.Send(new StartMeetingCommand
            {
                MeetingId = meetingId,
                UserId = CurrentUserId
            });

            return RedirectToAction(nameof(Room), new { id = meetingId });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid meetingId)
        {
            await mediator.Send(new CancelMeetingCommand
            {
                MeetingId = meetingId,
                UserId = CurrentUserId
            });

            TempData["Success"] = "Meeting cancelled.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(500 * 1024 * 1024)]          // 500 MB max
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public async Task<IActionResult> UploadRecording( IFormFile audio, Guid meetingId, CancellationToken ct)
        {
            if (audio == null || audio.Length == 0)
                return BadRequest("No audio file received.");

            var meeting = await mediator.Send(new GetMeetingByIdQuery { MeetingId = meetingId }, ct);
            if (meeting == null)
                return NotFound();

            // Only host or participants can upload
            if (meeting.HostId != CurrentUserId &&
                !meeting.Participants.Any(p => p.UserId == CurrentUserId))
                return Forbid();

            await recordingService.SaveAsync(meetingId, audio, ct);

            return Ok(new { message = "Recording saved." });
        }


        [HttpGet]
        public async Task<IActionResult> SearchUsers(string term, Guid meetingId)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
                return Json(new List<object>());

            var results = await mediator.Send(new SearchUsersToInviteQuery
            {
                Term = term,
                MeetingId = meetingId,
                HostId = CurrentUserId,
                Limit = 6
            });

            return Json(results);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(Guid meetingId, string invitedUserId)
        {
            await mediator.Send(new InviteUserCommand
            {
                MeetingId = meetingId,
                InvitedUserId = invitedUserId,
                InvitedByUserId = CurrentUserId
            });

            TempData["Success"] = "User invited successfully.";
            return RedirectToAction(nameof(Details), new { id = meetingId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RevokeInvitation(Guid invitationId, Guid meetingId)
        {
            await mediator.Send(new RevokeInvitationCommand
            {
                InvitationId = invitationId,
                HostId = CurrentUserId
            });

            TempData["Success"] = "Invitation revoked.";
            return RedirectToAction(nameof(Details), new { id = meetingId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(Guid invitationId, InvitationStatus response, Guid meetingId)
        {
            await mediator.Send(new RespondToInvitationCommand
            {
                InvitationId = invitationId,
                UserId = CurrentUserId,
                Response = response
            });

            TempData["Success"] = response == InvitationStatus.Accepted
                ? "Invitation accepted! You can now join the meeting."
            : "Invitation declined.";

            return RedirectToAction(nameof(Details), new { id = meetingId });
        }

        [HttpGet]
        public async Task<IActionResult> MyInvitations()
        {
            ViewData["Title"] = "My Invitations";

            var invitations = await mediator.Send(
                new GetUserPendingInvitationsQuery
                {
                    UserId = CurrentUserId
                });

            return View(invitations);
        }


        [HttpGet]
        public async Task<IActionResult> Room(Guid id)
        {
            var meeting = await mediator.Send(new GetMeetingByIdQuery { MeetingId = id });
            if (meeting == null) return NotFound();

            if (meeting.Status == MeetingStatus.Cancelled)
                return BadRequest("Meeting is cancelled.");

            if (meeting.Status == MeetingStatus.Ended)
                return RedirectToAction(nameof(Details), new { id });

            await mediator.Send(new JoinMeetingCommand
            {
                MeetingId = id,
                UserId = CurrentUserId
            });

            ViewData["Title"] = meeting.Title;
            ViewBag.IsHost = meeting.HostId == CurrentUserId;
            ViewBag.UserName = CurrentUserName;
            ViewBag.UserEmail = CurrentUserEmail;

            return View(meeting);
        }



        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Leave(Guid meetingId)
        {
            await mediator.Send(new LeaveMeetingCommand
            {
                MeetingId = meetingId,
                UserId = CurrentUserId
            });

            return RedirectToAction(nameof(Details), new { id = meetingId });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> End(Guid meetingId)
        {
            await mediator.Send(new EndMeetingCommand
            {
                MeetingId = meetingId,
                UserId = CurrentUserId
            });

            TempData["Success"] = "Meeting ended.";
            return RedirectToAction(nameof(Details), new { id = meetingId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Transcribe(Guid meetingId)
        {
            var meeting = await mediator.Send(new GetMeetingByIdQuery { MeetingId = meetingId });
            if (meeting == null) return NotFound();

            // Host only
            if (meeting.HostId != CurrentUserId)
                return Forbid();

            var result = await mediator.Send(new TranscribeMeetingCommand
            {
                MeetingId = meetingId,
                RequestedByUserId = CurrentUserId
            });

            if (result.Success)
                TempData["Success"] = "Transcription completed successfully!";
            else
                TempData["Error"] = $"Transcription failed: {result.Error}";

            return RedirectToAction(nameof(Details), new { id = meetingId });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Analyze(Guid meetingId)
        {
            var result = await mediator.Send(new AnalyzeMeetingCommand
            {
                MeetingId = meetingId,
                RequestedByUserId = CurrentUserId
            });

            TempData[result.Success ? "Success" : "Error"] = result.Success
                ? "AI analysis complete! Summary, tasks, and notes have been generated."
                : $"Analysis failed: {result.Error}";

            return RedirectToAction(nameof(Details), new { id = meetingId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PreviewAnalysis(Guid meetingId)
        {
            var result = await mediator.Send(new PreviewAnalysisCommand
            {
                MeetingId = meetingId,
                RequestedByUserId = CurrentUserId
            });

            if (!result.Success)
                return Json(new { success = false, error = result.Error });

            return Json(new { success = true, preview = result.Preview });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAnalysis([FromBody] ConfirmAnalysisRequest body)
        {
            var result = await mediator.Send(new ConfirmAnalysisCommand
            {
                MeetingId = body.MeetingId,
                RequestedByUserId = CurrentUserId,
                Summary = body.Summary,
                KeyInsights = body.KeyInsights,
                KeyDecisions = body.KeyDecisions,
                ApprovedTasks = body.ApprovedTasks,
                ApprovedNotes = body.ApprovedNotes,
            });

            return Json(new { success = result.Success, error = result.Error });
        }

        public class ConfirmAnalysisRequest
        {
            public Guid MeetingId { get; set; }
            public string Summary { get; set; } = string.Empty;
            public string KeyInsights { get; set; } = string.Empty;
            public List<string> KeyDecisions { get; set; } = [];
            public List<PreviewTaskDto> ApprovedTasks { get; set; } = [];
            public List<PreviewNoteDto> ApprovedNotes { get; set; } = [];
        }
    }
}