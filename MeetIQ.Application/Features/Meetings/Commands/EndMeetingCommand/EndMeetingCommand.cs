using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.EndMeetingCommand
{
    public class EndMeetingCommand : ICommand<bool>
    {
        public Guid MeetingId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}