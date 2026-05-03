using MediatR;
using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.JoinMeetingCommand
{
    public class JoinMeetingCommand : ICommand<Unit>
    {
        public Guid MeetingId { get; set; }
    }
}
