using MediatR;
using MeetIQ.Application.Features.Calendar.DTOs;

namespace MeetIQ.Application.Features.Calendar.Queries.GetCalendarEventByIdQuery
{
    public class GetCalendarEventByIdQuery : IRequest<CalendarEventDetailsDto?>
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}