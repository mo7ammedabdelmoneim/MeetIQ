using System.Security.Claims;
using MediatR;
using MeetIQ.Application.Features.Notifications.Queries.GetUnreadCountQuery;
using Microsoft.AspNetCore.Mvc;

namespace MeetIQ.Web.ViewComponents
{
    public class NotificationBellViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public NotificationBellViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int count = 0;
            if (!string.IsNullOrEmpty(userId))
            {
                count = await mediator.Send(
                    new GetUnreadCountQuery { UserId = userId });
            }

            return View(count);
        }
    }
}