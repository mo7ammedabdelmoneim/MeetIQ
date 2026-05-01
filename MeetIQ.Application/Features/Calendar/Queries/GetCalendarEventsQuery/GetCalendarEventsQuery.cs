using MediatR;
using MeetIQ.Application.Features.Calendar.DTOs;

namespace MeetIQ.Application.Features.Calendar.Queries.GetCalendarEventsQuery
{
    public class GetCalendarEventsQuery : IRequest<IReadOnlyList<CalendarEventDto>>
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime RangeFrom { get; set; }
        public DateTime RangeTo { get; set; }
    }
}
