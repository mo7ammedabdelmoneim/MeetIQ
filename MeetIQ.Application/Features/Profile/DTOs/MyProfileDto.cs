namespace MeetIQ.Application.Features.Profile.DTOs
{
    public class MyProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = string.Empty;

        // Activity stats
        public int TotalMeetings { get; set; }
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalNotes { get; set; }
        public int TotalFeedback { get; set; }
        public DateTime? LastMeetingAt { get; set; }
    }
}