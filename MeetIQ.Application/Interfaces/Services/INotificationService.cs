using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Services
{
    /// Thin wrapper around CreateNotificationCommand.
    /// Inject this wherever you need to fire a notification
    /// (handlers, background jobs, etc.)
    public interface INotificationService
    {
        Task NotifyAsync(
            string userId,
            NotificationType type,
            string title,
            string message,
            string? actionUrl = null,
            Guid? referenceId = null,
            NotificationReferenceType? referenceType = null);

        // Convenience overload for meeting events
        Task NotifyMeetingAsync(
            string userId,
            NotificationType type,
            string title,
            string message,
            Guid meetingId,
            string? actionUrl = null);
    }
}