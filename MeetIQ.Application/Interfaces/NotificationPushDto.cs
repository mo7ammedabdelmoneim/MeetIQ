using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Interfaces
{
    public class NotificationPushDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? ActionUrl { get; set; }
        public NotificationType Type { get; set; }
        public string CreatedAt { get; set; } = null!;
    }
}