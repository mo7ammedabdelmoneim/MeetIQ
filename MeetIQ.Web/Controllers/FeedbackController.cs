using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeetIQ.Application.Features.Feedback.Commands.CreateFeedbackCommand;
using MeetIQ.Application.Features.Feedback.Queries.GetMyFeedbacksQuery;
using MeetIQ.Web.ViewModels.Feedback;

namespace MeetIQ.Web.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly IMediator mediator;

        public FeedbackController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Report an Issue";
            return View(new CreateFeedbackViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Report an Issue";
                return View(model);
            }

            var command = new CreateFeedbackCommand
            {
                Type = model.Type,
                Message = model.Message,
                ReporterId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!
            };

            await mediator.Send(command);

            TempData["Success"] = "Your feedback has been submitted. Thank you!";
            return RedirectToAction(nameof(My));
        }

        [HttpGet]
        public async Task<IActionResult> My(int page = 1)
        {
            ViewData["Title"] = "My Feedback Reports";

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await mediator.Send(new GetMyFeedbacksQuery
            {
                UserId = userId!,
                Page = page
            });

            return View(result);
        }
    }
}