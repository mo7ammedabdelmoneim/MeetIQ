using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Services;
using MeetIQ.Domain.Entities;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.InviteUserCommand
{
    public class InviteUserCommandHandler : IRequestHandler<InviteUserCommand, Guid>
    {
        private readonly IMeetingRepository meetingRepository;
        private readonly IUserRepository userRepository;
        private readonly INotificationService notificationService;

        public InviteUserCommandHandler(
            IMeetingRepository meetingRepository,
            IUserRepository userRepository,
            INotificationService notificationService)
        {
            this.meetingRepository = meetingRepository;
            this.userRepository = userRepository;
            this.notificationService = notificationService;
        }

        public async Task<Guid> Handle(
            InviteUserCommand request,
            CancellationToken cancellationToken)
        {
            var meeting = await meetingRepository.GetAsync(x => x.Id == request.MeetingId);
            if (meeting == null)
                throw new NotFoundException("Meeting not found");

            if (meeting.HostId != request.InvitedByUserId)
                throw new UnauthorizedException("Only the host can invite users");

            if (meeting.Status == MeetingStatus.Ended || meeting.Status == MeetingStatus.Cancelled)
                throw new BadRequestException("Cannot invite to an ended or cancelled meeting");

            var invitedUser = await userRepository.GetAsync(x => x.Id == request.InvitedUserId);
            if (invitedUser == null)
                throw new NotFoundException("User not found");

            var invitedBy = await userRepository.GetAsync(x => x.Id == request.InvitedByUserId);

            var existing = await meetingRepository.GetInvitationAsync(
                request.MeetingId, request.InvitedUserId);

            if (existing != null)
                throw new BadRequestException("User has already been invited");

            var invitation = new MeetingInvitation
            {
                Id = Guid.NewGuid(),
                MeetingId = request.MeetingId,
                InvitedUserId = request.InvitedUserId,
                InvitedByUserId = request.InvitedByUserId,
                Status = InvitationStatus.Pending,
                InvitedAt = DateTime.UtcNow
            };

            await meetingRepository.AddInvitationAsync(invitation);
            await meetingRepository.SaveChangesAsync();

            // Notify 
            await notificationService.NotifyAsync(
                userId: request.InvitedUserId,
                type: NotificationType.InvitationReceived,
                title: "Meeting Invitation",
                message: $"{invitedBy?.FullName ?? "Someone"} invited you to \"{meeting.Title}\"",
                actionUrl: "/Meetings/MyInvitations",
                referenceId: invitation.Id,
                referenceType: NotificationReferenceType.Meeting);

            return invitation.Id;
        }
    }
}