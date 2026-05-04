using MediatR;
using MeetIQ.Application.Features.Dashboard.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Dashboard.Queries.GetDashboardQuery
{
    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDto>
    {
        private readonly IDashboardRepository dashboardRepository;

        public GetDashboardQueryHandler(IDashboardRepository dashboardRepository)
        {
            this.dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            // Sequential — same connection can't run parallel queries
            var stats = await dashboardRepository.GetStatsAsync(request.UserId);
            var upcoming = await dashboardRepository.GetUpcomingMeetingsAsync(request.UserId, count: 5);
            var recentTasks = await dashboardRepository.GetRecentTasksAsync(request.UserId, count: 5);
            var recentNotes = await dashboardRepository.GetRecentNotesAsync(request.UserId, count: 4);

            return new DashboardDto
            {
                UserFullName = request.UserFullName,
                Stats = stats,
                UpcomingMeetings = upcoming,
                RecentTasks = recentTasks,
                RecentNotes = recentNotes
            };
        }
    }
}