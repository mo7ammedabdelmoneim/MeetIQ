using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.CancelMeetingCommand
{
    public class CancelMeetingCommand : ICommand<bool>
    {
        public Guid MeetingId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}