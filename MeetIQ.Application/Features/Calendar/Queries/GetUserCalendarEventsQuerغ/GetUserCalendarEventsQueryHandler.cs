using MediatR;
using MeetIQ.Application.Features.Calendar.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Calendar.Queries.GetUserCalendarEvents
{
    public class GetUserCalendarEventsQueryHandler
        : IRequestHandler<GetUserCalendarEventsQuery, IEnumerable<CalendarEventSelectDto>>
    {
        private readonly ICalendarEventRepository _calendarEventRepository;

        public GetUserCalendarEventsQueryHandler(ICalendarEventRepository calendarEventRepository)
        {
            _calendarEventRepository = calendarEventRepository;
        }

        public async Task<IEnumerable<CalendarEventSelectDto>> Handle(
            GetUserCalendarEventsQuery request,
            CancellationToken cancellationToken)
        {
            return await _calendarEventRepository.GetUserEventsForSelectAsync(request.UserId);
        }
    }
}