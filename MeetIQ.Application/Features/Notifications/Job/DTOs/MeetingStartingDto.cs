namespace MeetIQ.Application.Features.Notifications.Job.DTOs
{
    public class MeetingStartingDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string HostId { get; set; } = null!;
        public DateTime ScheduledAt { get; set; }
        public List<string> ParticipantIds { get; set; } = [];
    }
}