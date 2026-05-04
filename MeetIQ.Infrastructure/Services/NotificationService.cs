using MediatR;
using MeetIQ.Application.Features.Notifications.Commands.CreateNotificationCommand;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMediator mediator;

        public NotificationService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task NotifyAsync(
            string userId,
            NotificationType type,
            string title,
            string message,
            string? actionUrl = null,
            Guid? referenceId = null,
            NotificationReferenceType? referenceType = null)
        {
            return mediator.Send(new CreateNotificationCommand
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                ActionUrl = actionUrl,
                ReferenceId = referenceId,
                ReferenceType = referenceType
            });
        }

        public Task NotifyMeetingAsync(
            string userId,
            NotificationType type,
            string title,
            string message,
            Guid meetingId,
            string? actionUrl = null)
        {
            return NotifyAsync(
                userId, type, title, message,
                actionUrl,
                meetingId,
                NotificationReferenceType.Meeting);
        }
    }
}