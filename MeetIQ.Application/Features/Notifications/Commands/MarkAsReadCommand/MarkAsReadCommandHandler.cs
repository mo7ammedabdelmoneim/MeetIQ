using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Notifications.Commands.MarkAsReadCommand
{
    public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand>
    {
        private readonly INotificationRepository notificationRepository;

        public MarkAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await notificationRepository
                .GetAsync(x => x.Id == request.NotificationId && x.UserId == request.UserId);

            if (notification is null)
                throw new NotFoundException("Notification not found");

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;

            notificationRepository.Update(notification);
            await notificationRepository.SaveChangesAsync();
        }
    }
}