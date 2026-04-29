using MediatR;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Tasks.DTOs;
using MeetIQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Tasks.Queries.GetTasksQuery
{
    public class GetTasksQuery : IRequest<PagedResult<TaskListItemDto>>
    {
        public string UserId { get; set; }

        public Domain.Enums.TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }

        public string? Search { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
