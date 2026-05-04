using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Features.Notifications.Commands.CreateNotificationCommand
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
    {
        private readonly INotificationRepository notificationRepository;

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task<Guid> Handle(
            CreateNotificationCommand request,
            CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Type = request.Type,
                Title = request.Title,
                Message = request.Message,
                ActionUrl = request.ActionUrl,
                ReferenceId = request.ReferenceId,
                ReferenceType = request.ReferenceType,
                IsRead = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await notificationRepository.AddAsync(notification);
            await notificationRepository.SaveChangesAsync();

            return notification.Id;
        }
    }
}