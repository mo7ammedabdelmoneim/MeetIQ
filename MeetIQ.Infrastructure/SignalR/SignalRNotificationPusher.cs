using MeetIQ.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace MeetIQ.Infrastructure.SignalR
{
    public class SignalRNotificationPusher : INotificationPusher
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public SignalRNotificationPusher(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task PushAsync(string userId, NotificationPushDto notification)
        {
            await hubContext.Clients
                .Group($"user-{userId}")
                .SendAsync("ReceiveNotification", notification);
        }

        public async Task PushUnreadCountAsync(string userId, int count)
        {
            await hubContext.Clients
                .Group($"user-{userId}")
                .SendAsync("UpdateUnreadCount", count);
        }
    }
}