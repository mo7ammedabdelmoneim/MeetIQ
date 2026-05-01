using MeetIQ.Application.Features.Calendar.DTOs;
using MeetIQ.Application.Features.Calendar.Queries.GetCalendarEventsQuery;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface ICalendarEventRepository : IRepository<CalendarEvent>
    {
        /// <summary>
        /// Returns events in a date range for the calendar grid.
        /// Uses Dapper + SqlKata (read path).
        /// </summary>
        Task<IReadOnlyList<CalendarEventDto>> GetEventsAsync(GetCalendarEventsQuery query);

        /// <summary>
        /// Returns full event detail, scoped to owner.
        /// Uses Dapper + SqlKata (read path).
        /// </summary>
        Task<CalendarEventDetailsDto?> GetByIdAsync(Guid id, string ownerId);
    }
}