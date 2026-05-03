using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MeetIQ.Application.Features.Meetings.Commands.CreateMeetingCommand
{
    public class CreateMeetingCommandHandler : IRequestHandler<CreateMeetingCommand, Guid>
    {
        private readonly IRepository<Meeting> _repo;
        private readonly IHttpContextAccessor _http;

        public CreateMeetingCommandHandler(IRepository<Meeting> repo, IHttpContextAccessor http)
        {
            _repo = repo;
            _http = http;
        }

        public async Task<Guid> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
        {
            var userId = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var meeting = new Meeting
            {
                Title = request.Title,
                ScheduledAt = request.ScheduledAt,
                HostId = userId,

                JitsiRoomId = $"meetiq-{Guid.NewGuid()}",

                Status = MeetingStatus.Scheduled
            };

            await _repo.AddAsync(meeting);
            await _repo.SaveChangesAsync();

            return meeting.Id;
        }
    }
}
