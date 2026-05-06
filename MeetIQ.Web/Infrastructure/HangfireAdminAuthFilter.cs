using Hangfire.Dashboard;
using MeetIQ.Application.Common.Constants;

namespace MeetIQ.Web.Infrastructure
{
    public class HangfireAdminAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.Identity?.IsAuthenticated == true
                   && httpContext.User.IsInRole(Roles.Admin);
        }
    }
}