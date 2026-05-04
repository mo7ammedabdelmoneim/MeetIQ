//using MeetIQ.Application.Interfaces.Repositories;
//using MeetIQ.Application.Services;
//using MeetIQ.Domain.Enums;

//namespace MeetIQ.Application.BackgroundJobs
//{
//    public class TaskOverdueJob
//    {
//        private readonly ITaskRepository taskRepository;
//        private readonly INotificationService notificationService;

//        public TaskOverdueJob(
//            ITaskRepository taskRepository,
//            INotificationService notificationService)
//        {
//            this.taskRepository = taskRepository;
//            this.notificationService = notificationService;
//        }

//        // Runs every day at 9 AM  →  RecurringJob.AddOrUpdate<TaskOverdueJob>("task-overdue", x => x.RunAsync(), "0 9 * * *");
//        public async Task RunAsync()
//        {
//            var overdueTasks = await taskRepository.GetOverdueTasksAsync();

//            foreach (var task in overdueTasks)
//            {
//                bool alreadyNotified = await taskRepository
//                    .HasRecentNotificationAsync(task.Id, NotificationType.TaskOverdue, hours: 20);

//                if (alreadyNotified) continue;

//                var daysLate = (int)(DateTime.UtcNow - task.DueDate!.Value).TotalDays;
//                var lateText = daysLate == 1 ? "1 day late" : $"{daysLate} days late";

//                await notificationService.NotifyAsync(
//                    userId: task.UserId,
//                    type: NotificationType.TaskOverdue,
//                    title: "Task Overdue",
//                    message: $"\"{task.Title}\" is {lateText}.",
//                    actionUrl: $"/Tasks/Details/{task.Id}",
//                    referenceId: task.Id,
//                    referenceType: NotificationReferenceType.Task
//                );
//            }
//        }
//    }
//}
