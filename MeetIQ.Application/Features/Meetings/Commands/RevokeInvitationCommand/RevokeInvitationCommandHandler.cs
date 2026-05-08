using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Meetings.Commands.RevokeInvitationCommand
{
    public class RevokeInvitationCommandHandler : IRequestHandler<RevokeInvitationCommand, bool>
    {
        private readonly IMeetingRepository meetingRepository;

        public RevokeInvitationCommandHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
        }

        public async Task<bool> Handle(
            RevokeInvitationCommand request,
            CancellationToken cancellationToken)
        {
            var invitation = await meetingRepository.GetInvitationByIdAsync(request.InvitationId);

            if (invitation == null)
                throw new NotFoundException("Invitation not found");

            var meeting = await meetingRepository.GetAsync(x => x.Id == invitation.MeetingId);

            if (meeting?.HostId != request.HostId)
                throw new UnauthorizedException("Only the host can revoke invitations");

            meetingRepository.DeleteInvitation(invitation);
            await meetingRepository.SaveChangesAsync();

            return true;
        }
    }
}