using MediatR;

namespace MeetIQ.Application.Features.Notifications.Queries.GetUnreadCountQuery
{
    public class GetUnreadCountQuery : IRequest<int>
    {
        public string UserId { get; set; } = null!;
    }
}