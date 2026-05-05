using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.CreateMeetingCommand
{
    public class CreateMeetingCommand : ICommand<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public string HostId { get; set; } = string.Empty;
    }
}