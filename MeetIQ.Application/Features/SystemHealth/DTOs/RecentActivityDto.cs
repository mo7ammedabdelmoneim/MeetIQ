namespace MeetIQ.Application.Features.SystemHealth.DTOs
{
    public class RecentActivityDto
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ActorName { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
    }
}