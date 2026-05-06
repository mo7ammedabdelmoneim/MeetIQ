using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Services;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Infrastructure.Jobs
{
    public class TaskOverdueJob
    {
        private readonly ITaskRepository taskRepository;
        private readonly INotificationService notificationService;
        private readonly ILogger<TaskOverdueJob> logger;

        public TaskOverdueJob(
            ITaskRepository taskRepository,
            INotificationService notificationService,
            ILogger<TaskOverdueJob> logger)
        {
            this.taskRepository = taskRepository;
            this.notificationService = notificationService;
            this.logger = logger;
        }

        // Runs every day at 9 AM UTC
        public async Task RunAsync()
        {
            logger.LogInformation("[TaskOverdueJob] Starting at {Time}", DateTime.UtcNow);

            var overdueTasks = await taskRepository.GetOverdueTasksAsync();

            logger.LogInformation("[TaskOverdueJob] Found {Count} overdue tasks", overdueTasks.Count);

            int notified = 0;

            foreach (var task in overdueTasks)
            {
                // one notification per task per day 
                bool alreadySent = await taskRepository
                    .WasNotifiedRecentlyAsync(task.Id, NotificationType.TaskOverdue, withinHours: 20);

                if (alreadySent) continue;

                var daysLate = (int)(DateTime.UtcNow - task.DueDate).TotalDays;
                var lateText = daysLate switch
                {
                    0 => "today",
                    1 => "yesterday",
                    _ => $"{daysLate} days ago"
                };

                await notificationService.NotifyAsync(
                    userId: task.UserId,
                    type: NotificationType.TaskOverdue,
                    title: "Task Overdue",
                    message: $"\"{task.Title}\" was due {lateText} and is still open.",
                    actionUrl: $"/Tasks/Details/{task.Id}",
                    referenceId: task.Id,
                    referenceType: NotificationReferenceType.Task
                );

                notified++;
            }

            logger.LogInformation("[TaskOverdueJob] Sent {Count} notifications", notified);
        }
    }
}