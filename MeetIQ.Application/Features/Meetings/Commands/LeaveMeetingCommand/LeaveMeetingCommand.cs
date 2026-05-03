using MediatR;
using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Meetings.Commands.LeaveMeetingCommand
{
    public class LeaveMeetingCommand : ICommand<Unit>
    {
        public Guid MeetingId { get; set; }
    }
}
