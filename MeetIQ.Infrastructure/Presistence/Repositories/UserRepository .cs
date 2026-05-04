using MeetIQ.Application.Common;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Users.DTOs;
using MeetIQ.Application.Features.Users.Queries.GetUsersQuery;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Infrastructure.Presistence.Repositories;
using MeetIQ.Infrastructure.Presistence;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, QueryFactory db)
            : base(context, db)
        {
        }

        public async Task<UserDetailsDto?> GetUserByIdAsync(string userId)
        {
            var user = await db.Query("AspNetUsers as u")
                .LeftJoin("AspNetUserRoles as ur", "ur.UserId", "u.Id")
                .LeftJoin("AspNetRoles as r", "r.Id", "ur.RoleId")
                .Select(
                    "u.Id",
                    "u.FullName",
                    "u.Email",
                    "u.AvatarUrl",
                    "u.IsActive",
                    "u.CreatedAt",
                    "r.Name as Role"
                )
                .Where("u.Id", userId)
                .FirstOrDefaultAsync<UserDetailsDto>();

            if (user == null) return null;

            // Counts via separate queries (SqlKata subqueries)
            user.MeetingsCount = await db.Query("Meetings")
                .Where("HostId", userId)
                .CountAsync<int>();

            user.TasksCount = await db.Query("TaskItems")
                .Where("UserId", userId)
                .Where("IsDeleted", false)
                .CountAsync<int>();

            user.NotesCount = await db.Query("Notes")
                .Where("AuthorId", userId)
                .CountAsync<int>();

            user.FeedbackCount = await db.Query("FeedbackReports")
                .Where("ReporterId", userId)
                .CountAsync<int>();

            user.LastMeetingAt = await db.Query("Meetings")
                .Where("HostId", userId)
                .OrderByDesc("ScheduledAt")
                .Select("ScheduledAt")
                .FirstOrDefaultAsync<DateTime?>();

            return user;
        }

        public async Task<PagedResult<UserListItemDto>> GetUsersAsync(GetUsersQuery query)
        {
            var baseQuery = db.Query("AspNetUsers as u")
                .LeftJoin("AspNetUserRoles as ur", "ur.UserId", "u.Id")
                .LeftJoin("AspNetRoles as r", "r.Id", "ur.RoleId")
                .LeftJoin(
                    db.Query("Meetings").SelectRaw("HostId, COUNT(*) as MeetingsCount").GroupBy("HostId").As("m"),
                    j => j.On("m.HostId", "u.Id")
                )
                .LeftJoin(
                    db.Query("TaskItems").Where("IsDeleted", false).SelectRaw("UserId, COUNT(*) as TasksCount").GroupBy("UserId").As("t"),
                    j => j.On("t.UserId", "u.Id")
                );

            // Filters
            if (!string.IsNullOrEmpty(query.Search))
                baseQuery.Where(q => q
                    .WhereLike("u.FullName", $"%{query.Search}%")
                    .OrWhereLike("u.Email", $"%{query.Search}%"));

            if (query.IsActive.HasValue)
                baseQuery.Where("u.IsActive", query.IsActive.Value);

            if (!string.IsNullOrEmpty(query.Role))
                baseQuery.Where("r.Name", query.Role);

            // Count
            var countQuery = baseQuery.Clone();
            var total = await countQuery.CountAsync<int>();

            // Fetch
            var items = await baseQuery
                .OrderByDesc("u.CreatedAt")
                .ForPage(query.Page, query.PageSize)
                .Select(
                    "u.Id",
                    "u.FullName",
                    "u.Email",
                    "u.AvatarUrl",
                    "u.IsActive",
                    "u.CreatedAt",
                    "r.Name as Role",
                    "m.MeetingsCount",
                    "t.TasksCount"
                )
                .GetAsync<UserListItemDto>();

            return new PagedResult<UserListItemDto>
            {
                Items = items.ToList(),
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }
    }
}