namespace MeetIQ.Domain.Entities
{
    public class MeetingSummary
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SummaryText { get; set; } = string.Empty;
        public string? KeyDecisions { get; set; }   // JSON array stored as text
        public bool IsEdited { get; set; } = false;
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; } = null!;
    }

   
}
