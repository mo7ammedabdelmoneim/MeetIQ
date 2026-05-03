using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Features.Calendar.Commands.CreateCalendarEventCommand
{
    public class CreateCalendarEventCommandHandler
        : IRequestHandler<CreateCalendarEventCommand, Guid>
    {
        private readonly ICalendarEventRepository _repo;
        private readonly IMeetingRepository _meetingRepo;

        public CreateCalendarEventCommandHandler(
            ICalendarEventRepository repo,
            IMeetingRepository meetingRepo)
        {
            _repo = repo;
            _meetingRepo = meetingRepo;
        }

        public async Task<Guid> Handle(
            CreateCalendarEventCommand request,
            CancellationToken cancellationToken)
        {
           
        if (request.MeetingId.HasValue)
            {
                var meeting = await _meetingRepo.GetAsync(
                    m => m.Id == request.MeetingId && m.HostId == request.OwnerId);
 
                if (meeting is null)
                    throw new NotFoundException("Meeting not found or access denied.");
             }

            var ev = new CalendarEvent
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Color = request.Color,
                OwnerId = request.OwnerId,
                MeetingId = request.MeetingId,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(ev);
            await _repo.SaveChangesAsync();
 
            return ev.Id;
        }
    }
}