using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptByIdQuery;
using MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptsQuery;

namespace MeetIQ.Web.Controllers
{
    [Authorize]
    public class TranscriptsController : Controller
    {
        private readonly IMediator mediator;
        public TranscriptsController(IMediator mediator) => this.mediator = mediator;
        private string CurrentUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Transcripts";
            var items = await mediator.Send(new GetTranscriptsQuery { UserId = CurrentUserId });
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewData["Title"] = "Transcript";
            var transcript = await mediator.Send(new GetTranscriptByIdQuery { TranscriptId = id });
            if (transcript == null) return NotFound();
            if (transcript.HostId != CurrentUserId &&
                !transcript.ParticipantIds.Contains(CurrentUserId))
                return Forbid();
            return View(transcript);
        }
    }
}