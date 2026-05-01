using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Calendar.Commands.DeleteCalendarEventCommand
{
    public class DeleteCalendarEventCommandHandler
        : IRequestHandler<DeleteCalendarEventCommand, Unit>
    {
        private readonly ICalendarEventRepository _repo;

        public DeleteCalendarEventCommandHandler(ICalendarEventRepository repo)
            => _repo = repo;

        public async Task<Unit> Handle(
            DeleteCalendarEventCommand request,
            CancellationToken cancellationToken)
        {
            var ev = await _repo.GetAsync(
                e => e.Id == request.Id && e.OwnerId == request.OwnerId);

            if (ev is null)
                throw new NotFoundException("Calendar event not found or access denied.");

            _repo.Delete(ev);
            await _repo.SaveChangesAsync();

            return Unit.Value;
        }
    }
}