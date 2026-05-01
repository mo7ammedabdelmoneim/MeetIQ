using MediatR;
using MeetIQ.Application.Features.Calendar.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Calendar.Queries.GetCalendarEventByIdQuery
{
    public class GetCalendarEventByIdQueryHandler
        : IRequestHandler<GetCalendarEventByIdQuery, CalendarEventDetailsDto?>
    {
        private readonly ICalendarEventRepository _repo;

        public GetCalendarEventByIdQueryHandler(ICalendarEventRepository repo)
            => _repo = repo;

        public async Task<CalendarEventDetailsDto?> Handle(
            GetCalendarEventByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id, request.UserId);
        }
    }
}