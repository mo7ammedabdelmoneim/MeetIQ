using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.RespondToInvitationCommand
{
    public class RespondToInvitationCommandHandler : IRequestHandler<RespondToInvitationCommand, bool>
    {
        private readonly IMeetingRepository meetingRepository;

        public RespondToInvitationCommandHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
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

            invitation.Status = request.Response;
            invitation.RespondedAt = DateTime.UtcNow;

            meetingRepository.UpdateInvitation(invitation);
            await meetingRepository.SaveChangesAsync();

            return true;
        }
    }
}