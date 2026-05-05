using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.CancelMeetingCommand
{
    public class CancelMeetingCommandHandler : IRequestHandler<CancelMeetingCommand, bool>
    {
        private readonly IMeetingRepository meetingRepository;

        public CancelMeetingCommandHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
        }

        public async Task<bool> Handle(
            CancelMeetingCommand request,
            CancellationToken cancellationToken)
        {
            var meeting = await meetingRepository.GetAsync(x => x.Id == request.MeetingId);

            if (meeting == null)
                throw new NotFoundException("Meeting not found");

            if (meeting.HostId != request.UserId)
                throw new UnauthorizedException("Only the host can cancel the meeting");

            if (meeting.Status == MeetingStatus.Ended || meeting.Status == MeetingStatus.Cancelled)
                throw new BadRequestException("Meeting is already ended or cancelled");

            meeting.Status = MeetingStatus.Cancelled;

            meetingRepository.Update(meeting);
            await meetingRepository.SaveChangesAsync();

            return true;
        }
    }
}