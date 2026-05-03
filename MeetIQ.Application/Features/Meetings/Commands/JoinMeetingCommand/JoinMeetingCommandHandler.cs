using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MeetIQ.Application.Features.Meetings.Commands.JoinMeetingCommand
{
    public class JoinMeetingCommandHandler : IRequestHandler<JoinMeetingCommand,Unit>
    {
        private readonly IMeetingParticipantRepository meetingParticipantRepository;
        private readonly IHttpContextAccessor http;

        public JoinMeetingCommandHandler(IMeetingParticipantRepository meetingParticipantRepository, IHttpContextAccessor http)
        {
            this.meetingParticipantRepository = meetingParticipantRepository;
            this.http = http;
        }

        public async Task<Unit> Handle(JoinMeetingCommand request, CancellationToken cancellationToken)
        {
            var userId = http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var participant = new MeetingParticipant
            {
                MeetingId = request.MeetingId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            };

            await meetingParticipantRepository.AddAsync(participant);
            await meetingParticipantRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
