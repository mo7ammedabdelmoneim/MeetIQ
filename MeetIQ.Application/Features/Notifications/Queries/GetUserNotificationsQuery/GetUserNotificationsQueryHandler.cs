using MediatR;
using MeetIQ.Application.Features.Notifications.Queries.GetNotificationsQuery;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Notifications.Queries.GetUserNotificationsQuery;
public class GetUserNotificationsQueryHandler
    : IRequestHandler<GetUserNotificationsQuery, List<NotificationDto>>
{
    private readonly INotificationRepository notificationRepository;

    public GetUserNotificationsQueryHandler(
        INotificationRepository notificationRepository)
    {
        this.notificationRepository = notificationRepository;
    }

    public async Task<List<NotificationDto>> Handle(
        GetUserNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        return await notificationRepository.GetUserNotificationsAsync(
            request.UserId,
            request.Page,
            request.PageSize);
    }
}