namespace MeetIQ.Application.Features.Profile.Queries.GetUserProfileQuery
{
    public class UserProfileDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Stats
        public int TotalMeetings { get; set; }
        public int TotalNotes { get; set; }
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalTranscripts { get; set; }
        public int TotalSummaries { get; set; }
        public double MeetingHours { get; set; }
    }
}