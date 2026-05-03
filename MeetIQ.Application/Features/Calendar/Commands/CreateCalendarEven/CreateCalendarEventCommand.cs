using MediatR;
using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Calendar.Commands.CreateCalendarEventCommand
{
    public class CreateCalendarEventCommand : ICommand<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Color { get; set; } = "#3B82F6";
        public string OwnerId { get; set; } = string.Empty;
        public Guid? MeetingId { get; set; }
    }
}