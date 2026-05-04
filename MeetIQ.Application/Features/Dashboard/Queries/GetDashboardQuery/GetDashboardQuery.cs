using MediatR;
using MeetIQ.Application.Features.Dashboard.DTOs;

namespace MeetIQ.Application.Features.Dashboard.Queries.GetDashboardQuery
{
    public class GetDashboardQuery : IRequest<DashboardDto>
    {
        public string UserId { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
    }
}