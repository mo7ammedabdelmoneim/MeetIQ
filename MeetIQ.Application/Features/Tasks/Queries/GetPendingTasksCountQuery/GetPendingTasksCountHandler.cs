using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MeetIQ.Application.Features.Tasks.Queries.GetPendingTasksCountQuery
{
    public class GetPendingTasksCountHandler
    : IRequestHandler<GetPendingTasksCountQuery, int>
    {
        private readonly ITaskRepository repo;
        private readonly IHttpContextAccessor _http;

        public GetPendingTasksCountHandler(ITaskRepository repo, IHttpContextAccessor http)
        {
            this.repo = repo;
            _http = http;
        }

        public async Task<int> Handle(GetPendingTasksCountQuery request, CancellationToken cancellationToken)
        {
            var userId = _http.HttpContext?.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return await repo.GetPendingTasksCount(userId);
        }
    }
}
