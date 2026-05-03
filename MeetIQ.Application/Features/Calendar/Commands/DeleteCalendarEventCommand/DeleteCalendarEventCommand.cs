using MediatR;
using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Calendar.Commands.DeleteCalendarEventCommand
{
    public class DeleteCalendarEventCommand : ICommand<Unit>
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
    }
}