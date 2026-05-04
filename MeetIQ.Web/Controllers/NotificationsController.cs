using System.Security.Claims;
using MediatR;
using MeetIQ.Application.Features.Notifications.Commands.DeleteNotificationCommand;
using MeetIQ.Application.Features.Notifications.Commands.MarkAllAsReadCommand;
using MeetIQ.Application.Features.Notifications.Commands.MarkAsReadCommand;
using MeetIQ.Application.Features.Notifications.Queries.GetNotificationsQuery;
using MeetIQ.Application.Features.Notifications.Queries.GetUnreadCountQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetIQ.Web.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly IMediator mediator;

        public NotificationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private string UserId =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await mediator.Send(new GetNotificationsQuery
            {
                UserId = UserId,
                Page = page,
                PageSize = 15
            });

            ViewData["Title"] = "Notifications";
            return View(result);
        }

        // AJAX – mark single as read, then redirect to ActionUrl
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(Guid id, string? returnUrl)
        {
            await mediator.Send(new MarkAsReadCommand
            {
                NotificationId = id,
                UserId = UserId
            });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead()
        {
            await mediator.Send(new MarkAllAsReadCommand { UserId = UserId });
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await mediator.Send(new DeleteNotificationCommand
            {
                NotificationId = id,
                UserId = UserId
            });

            return RedirectToAction(nameof(Index));
        }

        // AJAX endpoint – returns unread badge count (JSON)
        [HttpGet]
        public async Task<IActionResult> UnreadCount()
        {
            var count = await mediator.Send(
                new GetUnreadCountQuery { UserId = UserId });

            return Json(new { count });
        }
    }
}