using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Calendar.Commands.UpdateCalendarEventCommand
{
    // ── Handler ───────────────────────────────────────────────────────────────
    public class UpdateCalendarEventCommandHandler
        : IRequestHandler<UpdateCalendarEventCommand, Unit>
    {
        private readonly ICalendarEventRepository _repo;

        public UpdateCalendarEventCommandHandler(ICalendarEventRepository repo)
            => _repo = repo;

        public async Task<Unit> Handle(
            UpdateCalendarEventCommand request,
            CancellationToken cancellationToken)
        {
            var ev = await _repo.GetAsync(
                 e => e.Id == request.Id && e.OwnerId == request.OwnerId);

            if (ev is null)
                throw new NotFoundException("Calendar event not found or access denied.");

            ev.Title = request.Title;
            ev.Description = request.Description;
            ev.StartTime = request.StartTime;
            ev.EndTime = request.EndTime;
            ev.Color = request.Color;
            ev.MeetingId = request.MeetingId;

            _repo.Update(ev);
            await _repo.SaveChangesAsync();

            return Unit.Value;
        }
    }
}