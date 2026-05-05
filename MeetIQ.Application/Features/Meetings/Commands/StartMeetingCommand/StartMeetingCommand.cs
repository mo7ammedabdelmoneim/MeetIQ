using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.StartMeetingCommand
{
    public class StartMeetingCommand : ICommand<bool>
    {
        public Guid MeetingId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}