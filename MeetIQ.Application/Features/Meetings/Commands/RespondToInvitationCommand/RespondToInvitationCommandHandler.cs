using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Services;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.RespondToInvitationCommand
{
    public class RespondToInvitationCommandHandler : IRequestHandler<RespondToInvitationCommand, bool>
    {
        private readonly IMeetingRepository meetingRepository;
        private readonly IUserRepository userRepository;
        private readonly INotificationService notificationService;   

        public RespondToInvitationCommandHandler(
            IMeetingRepository meetingRepository,
            IUserRepository userRepository,
            INotificationService notificationService)
        {
            this.meetingRepository = meetingRepository;
            this.userRepository = userRepository;
            this.notificationService = notificationService;
        }

        public async Task<bool> Handle(
            RespondToInvitationCommand request,
            CancellationToken cancellationToken)
        {
            var invitation = await meetingRepository.GetInvitationByIdAsync(request.InvitationId);
            if (invitation == null)
                throw new NotFoundException("Invitation not found");

            if (invitation.InvitedUserId != request.UserId)
                throw new UnauthorizedException("You can only respond to your own invitations");

            if (invitation.Status != InvitationStatus.Pending)
                throw new BadRequestException("Invitation has already been responded to");

            var responder = await userRepository.GetAsync(x => x.Id == request.UserId);
            var meeting = await meetingRepository.GetAsync(x => x.Id == invitation.MeetingId);

            invitation.Status = request.Response;
            invitation.RespondedAt = DateTime.UtcNow;

            meetingRepository.UpdateInvitation(invitation);
            await meetingRepository.SaveChangesAsync();

            // Notify 
            var accepted = request.Response == InvitationStatus.Accepted;

            await notificationService.NotifyAsync(
                userId: meeting!.HostId,
                type: accepted ? NotificationType.InvitationAccepted
                                        : NotificationType.InvitationDeclined,
                title: accepted ? "Invitation Accepted" : "Invitation Declined",
                message: $"{responder?.FullName ?? "Someone"} {(accepted ? "accepted" : "declined")} your invitation to \"{meeting.Title}\"",
                actionUrl: $"/Meetings/Details/{meeting.Id}",
                referenceId: invitation.Id,
                referenceType: NotificationReferenceType.Meeting);

            return true;
        }
    }
}