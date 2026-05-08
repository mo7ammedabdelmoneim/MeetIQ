using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.InviteUserCommand
{
    public class InviteUserCommand : ICommand<Guid>
    {
        public Guid MeetingId { get; set; }
        public string InvitedUserId { get; set; } = string.Empty;
        public string InvitedByUserId { get; set; } = string.Empty;
    }
}