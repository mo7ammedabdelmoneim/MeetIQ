namespace MeetIQ.Application.Features.Notifications.Job.DTOs
{
    public class TaskDueDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime DueDate { get; set; }
    }
}