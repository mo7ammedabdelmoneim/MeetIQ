using MediatR;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Notifications.Queries.GetUnreadCountQuery
{
    public class GetUnreadCountQueryHandler : IRequestHandler<GetUnreadCountQuery, int>
    {
        private readonly INotificationRepository _repo;

        public GetUnreadCountQueryHandler(INotificationRepository repo) => _repo = repo;

        public Task<int> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
            => _repo.GetUnreadCountAsync(request.UserId);
    }
}