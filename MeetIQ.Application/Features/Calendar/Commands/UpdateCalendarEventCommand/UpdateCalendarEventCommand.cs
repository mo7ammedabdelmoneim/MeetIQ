using MediatR;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Calendar.Commands.UpdateCalendarEventCommand
{
    // ── Command ───────────────────────────────────────────────────────────────
    public class UpdateCalendarEventCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Color { get; set; } = "#3B82F6";
        public Guid? MeetingId { get; set; }
    }
}