namespace MeetIQ.Application.Features.Dashboard.DTOs
{
    public class DashboardDto
    {
        public DashboardStatsDto Stats { get; set; } = new();
        public List<UpcomingMeetingDto> UpcomingMeetings { get; set; } = [];
        public List<RecentTaskDto> RecentTasks { get; set; } = [];
        public List<RecentNoteDto> RecentNotes { get; set; } = [];
        public string UserFullName { get; set; } = string.Empty;
    }

}