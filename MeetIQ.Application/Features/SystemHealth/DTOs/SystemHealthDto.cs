namespace MeetIQ.Application.Features.SystemHealth.DTOs
{
    public class SystemHealthDto
    {
        // Users 
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int NewUsersThisMonth { get; set; }

        // Meetings 
        public int TotalMeetings { get; set; }
        public int MeetingsThisMonth { get; set; }
        public int LiveMeetingsNow { get; set; }

        // Tasks 
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CompletedTasks { get; set; }

        // AI 
        public int TotalTranscripts { get; set; }
        public int UnprocessedTranscripts { get; set; }
        public int TotalSummaries { get; set; }
        public int EditedSummaries { get; set; }

        // Notes 
        public int TotalNotes { get; set; }
        public int AiGeneratedNotes { get; set; }

        // Feedback 
        public int TotalFeedback { get; set; }
        public int OpenFeedback { get; set; }
        public int InReviewFeedback { get; set; }
        public int BugReports { get; set; }

        // Charts 
        public List<UserGrowthPointDto> UserGrowth { get; set; } = [];
        public List<MeetingActivityPointDto> MeetingActivity { get; set; } = [];

        // Recent Activity 
        public List<RecentActivityDto> RecentActivities { get; set; } = [];
}
}