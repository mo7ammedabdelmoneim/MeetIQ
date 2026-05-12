namespace MeetIQ.Domain.Entities
{
    public class MeetingSummary
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string SummaryText { get; set; } = string.Empty;

        public string? KeyInsights { get; set; }

        public string? KeyDecisions { get; set; }

        public bool IsEdited { get; set; } = false;
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; } = null!;
    }
}