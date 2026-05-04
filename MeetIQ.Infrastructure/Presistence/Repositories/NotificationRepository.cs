using MeetIQ.Application.Features.Notifications.Queries.GetNotificationsQuery;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Infrastructure.Presistence.Repositories;
using MeetIQ.Infrastructure.Presistence;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context, QueryFactory db)
            : base(context, db) { }

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(
            string userId, int page, int pageSize)
        {
            var items = await db.Query("Notifications")
                .Where("UserId", userId)
                .Where("IsDeleted", false)
                .OrderByDesc("CreatedAt")
                .ForPage(page, pageSize)
                .Select(
                    "Id",
                    "Title",
                    "Message",
                    "ActionUrl",
                    "IsRead",
                    "CreatedAt",
                    "Type",
                    "ReferenceId",
                    "ReferenceType")
                .GetAsync<NotificationDto>();

            return items.ToList();
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await db.Query("Notifications")
                .Where("UserId", userId)
                .Where("IsRead", false)
                .Where("IsDeleted", false)
                .CountAsync<int>();
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            await db.Query("Notifications")
                .Where("UserId", userId)
                .Where("IsRead", false)
                .Where("IsDeleted", false)
                .UpdateAsync(new
                {
                    IsRead = true,
                    ReadAt = DateTime.UtcNow
                });
        }

        public async Task<NotificationDto?> GetByIdAsync(Guid id)
        {
            return await db.Query("Notifications")
                .Where("Id", id)
                .Where("IsDeleted", false)
                .Select(
                    "Id",
                    "Title",
                    "Message",
                    "ActionUrl",
                    "IsRead",
                    "CreatedAt",
                    "Type",
                    "ReferenceId",
                    "ReferenceType")
                .FirstOrDefaultAsync<NotificationDto>();
        }
    }
}