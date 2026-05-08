using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.RevokeInvitationCommand
{
    public class RevokeInvitationCommand : ICommand<bool>
    {
        public Guid InvitationId { get; set; }
        public string HostId { get; set; } = string.Empty;
    }
}