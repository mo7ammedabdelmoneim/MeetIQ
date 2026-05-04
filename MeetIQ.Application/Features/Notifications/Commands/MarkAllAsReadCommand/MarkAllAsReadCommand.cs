using MediatR;

namespace MeetIQ.Application.Features.Notifications.Commands.MarkAllAsReadCommand
{
    public class MarkAllAsReadCommand : IRequest
    {
        public string UserId { get; set; } = null!;
    }
}