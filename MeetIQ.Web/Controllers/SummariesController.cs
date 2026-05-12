using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeetIQ.Application.Features.Summaries.Queries.GetSummariesQuery;
using MeetIQ.Application.Features.Summaries.Queries.GetSummaryByIdQuery;

namespace MeetIQ.Web.Controllers
{
    [Authorize]
    public class SummariesController : Controller
    {
        private readonly IMediator mediator;
        public SummariesController(IMediator mediator) => this.mediator = mediator;
        private string CurrentUserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Summaries";
            var items = await mediator.Send(new GetSummariesQuery { UserId = CurrentUserId });
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewData["Title"] = "AI Summary";
            var summary = await mediator.Send(new GetSummaryByIdQuery { SummaryId = id });
            if (summary == null) return NotFound();
            if (summary.HostId != CurrentUserId &&
                !summary.ParticipantIds.Contains(CurrentUserId))
                return Forbid();
            return View(summary);
        }
    }
}