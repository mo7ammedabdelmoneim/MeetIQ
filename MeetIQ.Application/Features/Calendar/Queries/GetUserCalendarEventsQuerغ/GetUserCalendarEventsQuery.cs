using MediatR;
using MeetIQ.Application.Features.Calendar.DTOs;

namespace MeetIQ.Application.Features.Calendar.Queries.GetUserCalendarEvents
{
    public class GetUserCalendarEventsQuery : IRequest<IEnumerable<CalendarEventSelectDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}