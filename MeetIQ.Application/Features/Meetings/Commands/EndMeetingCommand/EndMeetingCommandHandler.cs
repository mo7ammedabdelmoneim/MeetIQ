using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.EndMeetingCommand
{
    public class EndMeetingCommandHandler : IRequestHandler<EndMeetingCommand, bool>
    {
        private readonly IMeetingRepository meetingRepository;

        public EndMeetingCommandHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
        }

        public async Task<bool> Handle(
            EndMeetingCommand request,
            CancellationToken cancellationToken)
        {
            var meeting = await meetingRepository.GetAsync(x => x.Id == request.MeetingId);

            if (meeting == null)
                throw new NotFoundException("Meeting not found");

            if (meeting.HostId != request.UserId)
                throw new UnauthorizedException("Only the host can end the meeting");

            if (meeting.Status != MeetingStatus.InProgress)
                throw new BadRequestException("Only in-progress meetings can be ended");

            var endedAt = DateTime.UtcNow;
            meeting.Status = MeetingStatus.Ended;
            meeting.EndedAt = endedAt;

            meetingRepository.Update(meeting);

            await meetingRepository.MarkAllParticipantsLeftAsync(request.MeetingId, endedAt);

            await meetingRepository.SaveChangesAsync();

            return true;
        }
    }
}