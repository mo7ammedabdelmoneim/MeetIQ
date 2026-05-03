using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MeetIQ.Application.Features.Meetings.Commands.LeaveMeetingCommand
{
    public class LeaveMeetingCommandHandler : IRequestHandler<LeaveMeetingCommand,Unit>
    {
        private readonly IMeetingParticipantRepository meetingParticipantRepository;
        private readonly IHttpContextAccessor _http;

        public LeaveMeetingCommandHandler(IMeetingParticipantRepository meetingParticipantRepository , IHttpContextAccessor http)
        {
            this.meetingParticipantRepository = meetingParticipantRepository;
            _http = http;
        }

        public async Task<Unit> Handle(LeaveMeetingCommand request, CancellationToken cancellationToken)
        {
            var userId = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var participant = await meetingParticipantRepository.GetAsync(x =>
                x.MeetingId == request.MeetingId &&
                x.UserId == userId);

            if (participant != null)
            {
                participant.LeftAt = DateTime.UtcNow;
                meetingParticipantRepository.Update(participant);
                await meetingParticipantRepository.SaveChangesAsync();
            }

            return Unit.Value;
        }
    }
}
