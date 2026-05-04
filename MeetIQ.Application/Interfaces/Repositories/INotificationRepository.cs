using MeetIQ.Application.Features.Notifications.Queries.GetNotificationsQuery;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<List<NotificationDto>> GetUserNotificationsAsync(string userId, int page, int pageSize);
        Task<int> GetUnreadCountAsync(string userId);
        Task MarkAllAsReadAsync(string userId);
        Task<NotificationDto?> GetByIdAsync(Guid id);
    }
}