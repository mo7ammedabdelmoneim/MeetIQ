using MediatR;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Notifications.Commands.MarkAllAsReadCommand
{
    public class MarkAllAsReadCommandHandler : IRequestHandler<MarkAllAsReadCommand>
    {
        private readonly INotificationRepository notificationRepository;

        public MarkAllAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
        {
            await notificationRepository.MarkAllAsReadAsync(request.UserId);
        }
    }
}