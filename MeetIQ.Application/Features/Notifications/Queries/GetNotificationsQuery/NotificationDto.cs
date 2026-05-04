using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Notifications.Queries.GetNotificationsQuery
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? ActionUrl { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationType Type { get; set; }
        public Guid? ReferenceId { get; set; }
        public NotificationReferenceType? ReferenceType { get; set; }
    }
}