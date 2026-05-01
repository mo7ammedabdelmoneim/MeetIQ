namespace MeetIQ.Application.Features.Calendar.DTOs
{
    // ── List / Full-calendar feed ──────────────────────────────────────────────
    public class CalendarEventDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Color { get; set; } = "#3B82F6";
        public Guid? MeetingId { get; set; }
        public string? MeetingTitle { get; set; }
        public string OwnerId { get; set; } = string.Empty;
    }

    // ── Details page ──────────────────────────────────────────────────────────
    public class CalendarEventDetailsDto : CalendarEventDto
    {
        public DateTime CreatedAt { get; set; }
    }

    // ── Meetings dropdown helper ───────────────────────────────────────────────
    public class MeetingSelectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}