using MediatR;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Tasks.DTOs;
using MeetIQ.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.Queries.GetTasksQuery
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, PagedResult<TaskListItemDto>>
    {
        private readonly ITaskRepository taskRepository;

        public GetTasksQueryHandler(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task<PagedResult<TaskListItemDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            return await taskRepository.GetTasksAsync(request);
        }
    }
}
