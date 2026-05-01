using MediatR;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Calendar.Commands.DeleteCalendarEventCommand
{
    public class DeleteCalendarEventCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
    }
}