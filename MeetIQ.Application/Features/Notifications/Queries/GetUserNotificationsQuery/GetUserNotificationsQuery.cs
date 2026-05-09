using MediatR;
using MeetIQ.Application.Features.Notifications.Queries.GetNotificationsQuery;

namespace MeetIQ.Application.Features.Notifications.Queries.GetUserNotificationsQuery;

public class GetUserNotificationsQuery
    : IRequest<List<NotificationDto>>
{
    public string UserId { get; set; } = string.Empty;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 15;
}