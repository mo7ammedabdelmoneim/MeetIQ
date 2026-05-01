using MeetIQ.Application.Features.Calendar.DTOs;
using MeetIQ.Application.Features.Calendar.Queries.GetCalendarEventsQuery;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Infrastructure.Presistence;
using SqlKata.Execution;

namespace MeetIQ.Infrastructure.Presistence.Repositories
{
    public class CalendarEventRepository
        : Repository<CalendarEvent>, ICalendarEventRepository
    {
        public CalendarEventRepository(ApplicationDbContext context, QueryFactory db)
            : base(context, db) { }

        // ── READ: Dapper + SqlKata ─────────────────────────────────────────────

        public async Task<IReadOnlyList<CalendarEventDto>> GetEventsAsync(
            GetCalendarEventsQuery query)
        {
            var items = await db
                .Query("CalendarEvents as ce")
                .LeftJoin("Meetings as m", "m.Id", "ce.MeetingId")
                .Select(
                    "ce.Id",
                    "ce.Title",
                    "ce.Description",
                    "ce.StartTime",
                    "ce.EndTime",
                    "ce.Color",
                    "ce.MeetingId",
                    "m.Title as MeetingTitle",
                    "ce.OwnerId")
                .Where("ce.OwnerId", query.UserId)
                .Where("ce.IsDeleted", false)
                .WhereDate("ce.StartTime", ">=", query.RangeFrom)
                .WhereDate("ce.EndTime", "<=", query.RangeTo)
                .OrderBy("ce.StartTime")
                .GetAsync<CalendarEventDto>();

            return items.ToList().AsReadOnly();
        }

        public async Task<CalendarEventDetailsDto?> GetByIdAsync(Guid id, string ownerId)
        {
            return await db
                .Query("CalendarEvents as ce")
                .LeftJoin("Meetings as m", "m.Id", "ce.MeetingId")
                .Select(
                    "ce.Id",
                    "ce.Title",
                    "ce.Description",
                    "ce.StartTime",
                    "ce.EndTime",
                    "ce.Color",
                    "ce.MeetingId",
                    "m.Title as MeetingTitle",
                    "ce.OwnerId",
                    "ce.CreatedAt")
                .Where("ce.Id", id)
                .Where("ce.OwnerId", ownerId)
                .Where("ce.IsDeleted", false)
                .FirstOrDefaultAsync<CalendarEventDetailsDto>();
        }
    }
}