using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Services;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.EndMeetingCommand
{
    public class EndMeetingCommandHandler : IRequestHandler<EndMeetingCommand, bool>
    {
        private readonly IMeetingRepository meetingRepository;
        private readonly INotificationService notificationService;

        public EndMeetingCommandHandler(IMeetingRepository meetingRepository, INotificationService notificationService)
        {
            this.meetingRepository = meetingRepository;
            this.notificationService = notificationService;
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

            var invitedUserIds = await meetingRepository.GetInvitedUserIdsAsync(request.MeetingId);

            var notifyTasks = invitedUserIds.Select(uid =>
                notificationService.NotifyMeetingAsync(
                    userId: uid,
                    type: NotificationType.MeetingEnded,
                    title: "Meeting Ended",
                    message: $"\"{meeting.Title}\" has ended",
                    meetingId: meeting.Id,
                    actionUrl: $"/Meetings/Details/{meeting.Id}"));

            await Task.WhenAll(notifyTasks);

            return true;
        }
    }
}