using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeetIQ.Application.Features.SystemHealth.Queries.GetSystemHealthQuery;
using MeetIQ.Application.Common.Constants;

namespace MeetIQ.Web.Controllers.Admin
{
    [Authorize(Roles = Roles.Admin)]
    [Route("Admin/Dashboard")]
    public class AdminDashboardController : Controller
    {
        private readonly IMediator mediator;

        public AdminDashboardController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET /Admin/Dashboard
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "System Health";

            var dto = await mediator.Send(new GetSystemHealthQuery());
            return View("/Views/Admin/Dashboard/Index.cshtml", dto);
        }
    }
}