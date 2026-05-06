using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Notifications.Job.DTOs;
using MeetIQ.Application.Features.Tasks.DTOs;
using MeetIQ.Application.Features.Tasks.Queries.GetTasksQuery;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface ITaskRepository:IRepository<TaskItem>
    {
        Task<TaskDetailsDto?> GetByIdAsync(Guid id);
        Task<PagedResult<TaskListItemDto>> GetTasksAsync(GetTasksQuery query);
        Task<int> GetPendingTasksCount(string userId);

        Task<List<TaskDueDto>> GetTasksDueBetweenAsync(DateTime from, DateTime to);
        Task<List<TaskDueDto>> GetOverdueTasksAsync();
        Task<bool> WasNotifiedRecentlyAsync(Guid taskId, NotificationType type, int withinHours);

    }
}
