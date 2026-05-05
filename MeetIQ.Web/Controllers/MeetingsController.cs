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

namespace MeetIQ.Web.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
        private readonly IMediator mediator;
        private readonly IJitsiTokenService jitsiTokenService;

        public MeetingsController(IMediator mediator, IJitsiTokenService jitsiTokenService)
        {
            this.mediator = mediator;
            this.jitsiTokenService = jitsiTokenService;
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

        // Jitsi embedded room
        //[HttpGet]
        //public async Task<IActionResult> Room(Guid id)
        //{
        //    var meeting = await mediator.Send(new GetMeetingByIdQuery { MeetingId = id });
        //    if (meeting == null) return NotFound();

        //    if (meeting.Status == MeetingStatus.Cancelled)
        //        return BadRequest("Meeting is cancelled.");

        //    if (meeting.Status == MeetingStatus.Ended)
        //        return RedirectToAction(nameof(Details), new { id });

        //    var isHost = meeting.HostId == CurrentUserId;

        //    // Record participant join
        //    await mediator.Send(new JoinMeetingCommand
        //    {
        //        MeetingId = id,
        //        UserId = CurrentUserId
        //    });

        //    // Generate Jitsi JWT (only needed for self-hosted / JaaS)
        //    var token = jitsiTokenService.GenerateToken(
        //        roomId: meeting.JitsiRoomId,
        //        userId: CurrentUserId,
        //        userName: CurrentUserName,
        //        userEmail: CurrentUserEmail,
        //        isModerator: isHost
        //    );

        //    ViewData["Title"] = meeting.Title;
        //    ViewBag.Meeting = meeting;
        //    ViewBag.IsHost = isHost;
        //    ViewBag.JitsiToken = token;
        //    ViewBag.UserName = CurrentUserName;
        //    ViewBag.UserEmail = CurrentUserEmail;

        //    return View(meeting);
        //}


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
    }
}