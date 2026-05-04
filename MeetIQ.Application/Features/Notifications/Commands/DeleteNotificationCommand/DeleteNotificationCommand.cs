using MediatR;

namespace MeetIQ.Application.Features.Notifications.Commands.DeleteNotificationCommand
{
    public class DeleteNotificationCommand : IRequest
    {
        public Guid NotificationId { get; set; }
        public string UserId { get; set; } = null!;
    }
}