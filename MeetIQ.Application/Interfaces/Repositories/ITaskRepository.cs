using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Tasks.DTOs;
using MeetIQ.Application.Features.Tasks.Queries.GetTasksQuery;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface ITaskRepository:IRepository<TaskItem>
    {
        Task<TaskDetailsDto?> GetByIdAsync(Guid id);
        Task<PagedResult<TaskListItemDto>> GetTasksAsync(GetTasksQuery query);
        Task<int> GetPendingTasksCount(string userId);

    }
}
