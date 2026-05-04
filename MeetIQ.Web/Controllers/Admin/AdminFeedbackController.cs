using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeetIQ.Application.Features.Feedback.Commands.UpdateFeedbackStatusCommand;
using MeetIQ.Application.Features.Feedback.Queries.GetFeedbackByIdQuery;
using MeetIQ.Application.Features.Feedback.Queries.GetFeedbacksQuery;
using MeetIQ.Domain.Enums;
using MeetIQ.Web.ViewModels.Feedback;
using MeetIQ.Application.Common.Constants;

namespace MeetIQ.Web.Controllers.Admin
{
    [Authorize(Roles = Roles.Admin)]
    [Route("Admin/Feedback")]
    public class AdminFeedbackController : Controller
    {
        private readonly IMediator mediator;

        public AdminFeedbackController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET /Admin/Feedback
        [HttpGet("")]
        public async Task<IActionResult> Index(
            FeedbackStatus? status,
            FeedbackType? type,
            string? search,
            int page = 1)
        {
            ViewData["Title"] = "Feedback Reports";

            var result = await mediator.Send(new GetFeedbacksQuery
            {
                Status = status,
                Type = type,
                Search = search,
                Page = page
            });

            ViewBag.CurrentStatus = status;
            ViewBag.CurrentType = type;
            ViewBag.CurrentSearch = search;

            return View("/Views/Admin/AdminFeedback/Index.cshtml", result);
        }

        // GET /Admin/Feedback/Details/{id}
        [HttpGet("Details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewData["Title"] = "Feedback Details";

            var feedback = await mediator.Send(new GetFeedbackByIdQuery { Id = id });

            if (feedback == null)
                return NotFound();

            return View("/Views/Admin/AdminFeedback/FeedbackDetails.cshtml", feedback);
        }

        // POST /Admin/Feedback/UpdateStatus
        [HttpPost("UpdateStatus"), ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateFeedbackStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Details), new { id = model.FeedbackId });

            await mediator.Send(new UpdateFeedbackStatusCommand
            {
                FeedbackId = model.FeedbackId,
                NewStatus = model.NewStatus
            });

            TempData["Success"] = "Feedback status updated successfully.";
            return RedirectToAction(nameof(Details), new { id = model.FeedbackId });
        }
    }
}