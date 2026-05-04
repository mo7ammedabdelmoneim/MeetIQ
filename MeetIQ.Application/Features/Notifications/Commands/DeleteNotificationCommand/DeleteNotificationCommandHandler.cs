using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Notifications.Commands.DeleteNotificationCommand
{
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
    {
        private readonly INotificationRepository notificationRepository;

        public DeleteNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await notificationRepository
                .GetAsync(x => x.Id == request.NotificationId && x.UserId == request.UserId);

            if (notification is null)
                throw new NotFoundException("Notification not found");

            // Soft delete
            notification.IsDeleted = true;
            notificationRepository.Update(notification);
            await notificationRepository.SaveChangesAsync();
        }
    }
}