namespace MeetIQ.Application.Features.Dashboard.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalMeetingsHosted { get; set; }
        public int UpcomingMeetingsCount { get; set; }
        public int PendingTasksCount { get; set; }
        public int TotalNotesCount { get; set; }
        public int UnprocessedTranscriptsCount { get; set; }
    }
}