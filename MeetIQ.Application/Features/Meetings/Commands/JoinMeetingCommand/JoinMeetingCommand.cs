using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.JoinMeetingCommand
{
    public class JoinMeetingCommand : ICommand<bool>
    {
        public Guid MeetingId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}