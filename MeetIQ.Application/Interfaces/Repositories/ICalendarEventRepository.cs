using MeetIQ.Application.Features.Calendar.DTOs;
using MeetIQ.Application.Features.Calendar.Queries.GetCalendarEventsQuery;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface ICalendarEventRepository : IRepository<CalendarEvent>
    {
        Task<IReadOnlyList<CalendarEventDto>> GetEventsAsync(GetCalendarEventsQuery query);
        Task<CalendarEventDetailsDto?> GetByIdAsync(Guid id, string ownerId);
        Task<IEnumerable<CalendarEventSelectDto>> GetUserEventsForSelectAsync(string userId);
    }
}