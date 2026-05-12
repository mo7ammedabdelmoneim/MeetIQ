using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Services;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Infrastructure.Jobs
{
    public class TaskDueSoonJob
    {
        private readonly ITaskRepository taskRepository;
        private readonly INotificationService notificationService;
        private readonly ILogger<TaskDueSoonJob> logger;

        public TaskDueSoonJob(
            ITaskRepository taskRepository,
            INotificationService notificationService,
            ILogger<TaskDueSoonJob> logger)
        {
            this.taskRepository = taskRepository;
            this.notificationService = notificationService;
            this.logger = logger;
        }

        // Runs every hour
        // Catches tasks due in the next 23-25h window
        public async Task RunAsync()
        {
            logger.LogInformation("[TaskDueSoonJob] Starting at {Time}", DateTime.UtcNow);

            var windowStart = DateTime.UtcNow.AddHours(23);
            var windowEnd = DateTime.UtcNow.AddHours(25);

            var tasks = await taskRepository.GetTasksDueBetweenAsync(windowStart, windowEnd);

            logger.LogInformation("[TaskDueSoonJob] Found {Count} tasks due soon", tasks.Count);

            int notified = 0;

            foreach (var task in tasks)
            {
                // don't spam if already sent within 20h 
                bool alreadySent = await taskRepository
                    .WasNotifiedRecentlyAsync(task.Id, NotificationType.TaskDueSoon, withinHours: 20);

                if (alreadySent) continue;

                var hoursLeft = (int)(task.DueDate - DateTime.UtcNow).TotalHours;
                var timeText = hoursLeft <= 1 ? "less than an hour" : $"~{hoursLeft} hours";

                await notificationService.NotifyAsync(
                    userId: task.UserId,
                    type: NotificationType.TaskDueSoon,
                    title: "Task Due Soon",
                    message: $"\"{task.Title}\" is due in {timeText}.",
                    actionUrl: $"/Tasks/Details/{task.Id}",
                    referenceId: task.Id,
                    referenceType: NotificationReferenceType.Task
                );

                notified++;
            }

            logger.LogInformation("[TaskDueSoonJob] Sent {Count} notifications", notified);
        }
    }
}