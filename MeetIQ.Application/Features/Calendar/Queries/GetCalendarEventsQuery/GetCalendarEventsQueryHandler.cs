using MediatR;
using MeetIQ.Application.Features.Calendar.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Calendar.Queries.GetCalendarEventsQuery
{
    public class GetCalendarEventsQueryHandler
        : IRequestHandler<GetCalendarEventsQuery, IReadOnlyList<CalendarEventDto>>
    {
        private readonly ICalendarEventRepository _repo;

        public GetCalendarEventsQueryHandler(ICalendarEventRepository repo)
            => _repo = repo;

        public async Task<IReadOnlyList<CalendarEventDto>> Handle(
            GetCalendarEventsQuery request,
            CancellationToken cancellationToken)
        {
            return await _repo.GetEventsAsync(request);
        }
    }
}