using MediatR;
using MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MeetIQ.Interface.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
        private readonly IMediator mediator;

        public MeetingsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var meetings = await mediator.Send(new GetUserMeetingsQuery
            {
                UserId = userId,
                FromDate = fromDate,
                ToDate = toDate
            });

            return View(meetings);
        }
    }
}
