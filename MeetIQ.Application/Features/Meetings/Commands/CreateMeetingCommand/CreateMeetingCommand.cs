using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.CreateMeetingCommand
{
    public class CreateMeetingCommand : ICommand<Guid>
    {
        public string Title { get; set; }
        public DateTime ScheduledAt { get; set; }
    }
}
