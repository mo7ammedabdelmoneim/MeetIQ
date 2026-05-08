namespace MeetIQ.Application.Interfaces
{
    public interface INotificationPusher
    {
        Task PushAsync(string userId, NotificationPushDto notification);
        Task PushUnreadCountAsync(string userId, int count);
    }
}