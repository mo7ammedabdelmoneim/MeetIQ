using MediatR;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Meetings.Commands.LeaveMeetingCommand
{
    public class LeaveMeetingCommandHandler : IRequestHandler<LeaveMeetingCommand, bool>
    {
        private readonly IMeetingRepository meetingRepository;

        public LeaveMeetingCommandHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
        }

        public async Task<bool> Handle(
            LeaveMeetingCommand request,
            CancellationToken cancellationToken)
        {
            var participant = await meetingRepository.GetParticipantAsync(
                request.MeetingId, request.UserId);

            if (participant == null)
                return true;

            participant.LeftAt = DateTime.UtcNow;
            meetingRepository.UpdateParticipant(participant);
            await meetingRepository.SaveChangesAsync();

            return true;
        }
    }
}