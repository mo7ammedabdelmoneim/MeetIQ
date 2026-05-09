using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Services;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.StartMeetingCommand
{
    public class StartMeetingCommandHandler : IRequestHandler<StartMeetingCommand, bool>
    {
        private readonly IMeetingRepository meetingRepository;
        private readonly INotificationService notificationService;

        public StartMeetingCommandHandler(
            IMeetingRepository meetingRepository,
            INotificationService notificationService)
        {
            this.meetingRepository = meetingRepository;
            this.notificationService = notificationService;
        }

        public async Task<bool> Handle(
            StartMeetingCommand request,
            CancellationToken cancellationToken)
        {
            var meeting = await meetingRepository.GetAsync(x => x.Id == request.MeetingId);

            if (meeting == null)
                throw new NotFoundException("Meeting not found");

            if (meeting.HostId != request.UserId)
                throw new UnauthorizedException("Only the host can start the meeting");

            if (meeting.Status != MeetingStatus.Scheduled)
                throw new BadRequestException("Meeting cannot be started in its current status");

            meeting.Status = MeetingStatus.InProgress;
            meeting.StartedAt = DateTime.UtcNow;

            meetingRepository.Update(meeting);
            await meetingRepository.SaveChangesAsync();

            var invitedUserIds = await meetingRepository.GetInvitedUserIdsAsync(request.MeetingId);

            var notifyTasks = invitedUserIds.Select(uid =>
                notificationService.NotifyMeetingAsync(
                    userId: uid,
                    type: NotificationType.YouWereAddedToMeeting,
                    title: "Meeting Started!",
                    message: $"\"{meeting.Title}\" is now live. Join now!",
                    meetingId: meeting.Id,
                    actionUrl: $"/Meetings/Room/{meeting.Id}"));

            await Task.WhenAll(notifyTasks);

            return true;
        }
    }
}