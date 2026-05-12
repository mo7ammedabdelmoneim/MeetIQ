using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Features.Meetings.Hubs;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Services;
using MeetIQ.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace MeetIQ.Application.Features.Meetings.Commands.EndMeetingCommand
{
    public class EndMeetingCommandHandler : IRequestHandler<EndMeetingCommand, bool>
    {
        private readonly IMeetingRepository meetingRepository;
        private readonly INotificationService notificationService;
        private readonly IHubContext<MeetingHub> hubContext;

        public EndMeetingCommandHandler(IMeetingRepository meetingRepository, INotificationService notificationService, IHubContext<MeetingHub> hubContext)
        {
            this.meetingRepository = meetingRepository;
            this.notificationService = notificationService;
            this.hubContext = hubContext;
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

            await hubContext.Clients
                    .Group($"meeting-{meeting.Id}")
                    .SendAsync("MeetingEnded");

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