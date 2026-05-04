using MeetIQ.Application.Features.Dashboard.DTOs;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardStatsDto> GetStatsAsync(string userId);
        Task<List<UpcomingMeetingDto>> GetUpcomingMeetingsAsync(string userId, int count = 5);
        Task<List<RecentTaskDto>> GetRecentTasksAsync(string userId, int count = 5);
        Task<List<RecentNoteDto>> GetRecentNotesAsync(string userId, int count = 5);
    }
}