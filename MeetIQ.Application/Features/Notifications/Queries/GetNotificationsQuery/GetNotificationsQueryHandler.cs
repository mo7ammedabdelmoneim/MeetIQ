using MediatR;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Notifications.Queries.GetNotificationsQuery
{
    public class GetNotificationsQueryHandler
        : IRequestHandler<GetNotificationsQuery, PagedResult<NotificationDto>>
    {
        private readonly INotificationRepository notificationRepository;

        public GetNotificationsQueryHandler(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task<PagedResult<NotificationDto>> Handle(
            GetNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            var items = await notificationRepository
                .GetUserNotificationsAsync(request.UserId, request.Page, request.PageSize);

            var total = items.Count; 

            return new PagedResult<NotificationDto>
            {
                Items = items,
                TotalCount = total,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}