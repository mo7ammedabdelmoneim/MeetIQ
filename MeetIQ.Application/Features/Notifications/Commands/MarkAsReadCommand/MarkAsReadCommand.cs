using MediatR;

namespace MeetIQ.Application.Features.Notifications.Commands.MarkAsReadCommand
{
    public class MarkAsReadCommand : IRequest
    {
        public Guid NotificationId { get; set; }
        public string UserId { get; set; } = null!;
    }
}