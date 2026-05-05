using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.JoinMeetingCommand
{
    public class JoinMeetingCommandHandler : IRequestHandler<JoinMeetingCommand, bool>
    {
        private readonly IMeetingRepository meetingRepository;

        public JoinMeetingCommandHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
        }

        public async Task<bool> Handle(
            JoinMeetingCommand request,
            CancellationToken cancellationToken)
        {
            var meeting = await meetingRepository.GetAsync(x => x.Id == request.MeetingId);

            if (meeting == null)
                throw new NotFoundException("Meeting not found");

            if (meeting.Status == MeetingStatus.Cancelled)
                throw new BadRequestException("Cannot join a cancelled meeting");

            if (meeting.Status == MeetingStatus.Ended)
                throw new BadRequestException("Cannot join a meeting that has ended");

            // Upsert participant — avoid duplicates
            var existing = await meetingRepository.GetParticipantAsync(request.MeetingId, request.UserId);

            if (existing == null)
            {
                var participant = new MeetingParticipant
                {
                    Id = Guid.NewGuid(),
                    MeetingId = request.MeetingId,
                    UserId = request.UserId,
                    JoinedAt = DateTime.UtcNow
                };
                await meetingRepository.AddParticipantAsync(participant);
            }
            else
            {
                // Re-joining — reset LeftAt
                existing.LeftAt = null;
                existing.JoinedAt = DateTime.UtcNow;
                meetingRepository.UpdateParticipant(existing);
            }

            await meetingRepository.SaveChangesAsync();
            return true;
        }
    }
}