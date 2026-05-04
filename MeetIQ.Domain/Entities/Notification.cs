using MeetIQ.Domain.Enums;
namespace MeetIQ.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public NotificationType Type { get; set; }

        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;

        // Optional deep-link (e.g. /Meetings/Details/{id})
        public string? ActionUrl { get; set; }

        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        // Polymorphic reference to the triggering entity
        public Guid? ReferenceId { get; set; }
        public NotificationReferenceType? ReferenceType { get; set; }

        // Navigation
        public ApplicationUser User { get; set; } = null!;
    }
}