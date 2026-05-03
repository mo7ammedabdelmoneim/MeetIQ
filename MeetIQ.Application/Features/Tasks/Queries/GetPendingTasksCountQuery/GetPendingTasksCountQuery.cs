using MediatR;

namespace MeetIQ.Application.Features.Tasks.Queries.GetPendingTasksCountQuery
{
    public class GetPendingTasksCountQuery : IRequest<int>
    {
        public string UserId { get; set; }
    }
}
