using MeetIQ.Application.Interfaces;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.RespondToInvitationCommand
{
    public class RespondToInvitationCommand : ICommand<bool>
    {
        public Guid InvitationId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public InvitationStatus Response { get; set; } 
    }
}