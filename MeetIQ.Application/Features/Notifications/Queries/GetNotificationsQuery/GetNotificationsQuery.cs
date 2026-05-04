using MediatR;
using MeetIQ.Application.Common.Results;

namespace MeetIQ.Application.Features.Notifications.Queries.GetNotificationsQuery
{
    public class GetNotificationsQuery : IRequest<PagedResult<NotificationDto>>
    {
        public string UserId { get; set; } = null!;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}