using MediatR;
using MeetIQ.Application.Features.Notifications.Commands.CreateNotificationCommand;
using MeetIQ.Application.Interfaces;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMediator mediator;
        private readonly INotificationPusher pusher;
        private readonly INotificationRepository notificationRepository;

        public NotificationService(
            IMediator mediator,
            INotificationPusher pusher,
            INotificationRepository notificationRepository)
        {
            this.mediator = mediator;
            this.pusher = pusher;
            this.notificationRepository = notificationRepository;
        }

        public async Task NotifyAsync(
            string userId,
            NotificationType type,
            string title,
            string message,
            string? actionUrl = null,
            Guid? referenceId = null,
            NotificationReferenceType? referenceType = null)
        {
            // 1 — Save to DB
            var id = await mediator.Send(new CreateNotificationCommand
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                ActionUrl = actionUrl,
                ReferenceId = referenceId,
                ReferenceType = referenceType
            });

            // 2 — Push live notification via SignalR
            await pusher.PushAsync(userId, new NotificationPushDto
            {
                Id = id,
                Title = title,
                Message = message,
                ActionUrl = actionUrl,
                Type = type,
                CreatedAt = "just now"
            });

            // 3 — Push updated unread count
            var unreadCount = await notificationRepository.GetUnreadCountAsync(userId);
            await pusher.PushUnreadCountAsync(userId, unreadCount);
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