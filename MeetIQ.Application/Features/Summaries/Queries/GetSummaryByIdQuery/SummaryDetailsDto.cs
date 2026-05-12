namespace MeetIQ.Application.Features.Summaries.Queries.GetSummaryByIdQuery
{
    public class SummaryDetailsDto
    {
        public Guid Id { get; set; }
        public Guid MeetingId { get; set; }
        public string MeetingTitle { get; set; } = string.Empty;
        public string HostId { get; set; } = string.Empty;
        public string SummaryText { get; set; } = string.Empty;
        public string? KeyInsights { get; set; }
        public string? KeyDecisions { get; set; }   // JSON array string
        public bool IsEdited { get; set; }
        public DateTime GeneratedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> ParticipantIds { get; set; } = [];
    }
}