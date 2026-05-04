using MeetIQ.Application.Interfaces;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Notifications.Commands.CreateNotificationCommand
{
    public class CreateNotificationCommand : ICommand<Guid>
    {
        public string UserId { get; set; } = null!;
        public NotificationType Type { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? ActionUrl { get; set; }
        public Guid? ReferenceId { get; set; }
        public NotificationReferenceType? ReferenceType { get; set; }
    }
}