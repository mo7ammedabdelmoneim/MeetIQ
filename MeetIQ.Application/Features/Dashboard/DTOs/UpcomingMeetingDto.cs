namespace MeetIQ.Application.Features.Dashboard.DTOs
{
    public class UpcomingMeetingDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsHost { get; set; }
        public int ParticipantsCount { get; set; }
        public string? JitsiRoomId { get; set; }
    }
}
