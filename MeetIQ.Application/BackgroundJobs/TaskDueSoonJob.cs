//using MeetIQ.Application.Interfaces.Repositories;
//using MeetIQ.Application.Services;
//using MeetIQ.Domain.Enums;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MeetIQ.Application.BackgroundJobs
//{
//    // ── Hangfire Recurring Jobs ─────────────────────────────────────────

//    public class TaskDueSoonJob
//    {
//        private readonly ITaskRepository taskRepository;
//        private readonly INotificationService notificationService;

//        public TaskDueSoonJob(
//            ITaskRepository taskRepository,
//            INotificationService notificationService)
//        {
//            this.taskRepository = taskRepository;
//            this.notificationService = notificationService;
//        }

//        // Runs every hour  →  RecurringJob.AddOrUpdate<TaskDueSoonJob>("task-due-soon", x => x.RunAsync(), "0 * * * *");
//        public async Task RunAsync()
//        {
//            var windowStart = DateTime.UtcNow.AddHours(23);
//            var windowEnd = DateTime.UtcNow.AddHours(25);   // 24h window ±1h

//            var dueSoonTasks = await taskRepository.GetTasksDueBetweenAsync(windowStart, windowEnd);

//            foreach (var task in dueSoonTasks)
//            {
//                // Skip if we already sent this notification today
//                bool alreadyNotified = await taskRepository
//                    .HasRecentNotificationAsync(task.Id, NotificationType.TaskDueSoon, hours: 20);

//                if (alreadyNotified) continue;

//                await notificationService.NotifyAsync(
//                    userId: task.UserId,
//                    type: NotificationType.TaskDueSoon,
//                    title: "Task Due Soon",
//                    message: $"\"{task.Title}\" is due in ~24 hours.",
//                    actionUrl: $"/Tasks/Details/{task.Id}",
//                    referenceId: task.Id,
//                    referenceType: NotificationReferenceType.Task
//                );
//            }
//        }
//    }
//}
