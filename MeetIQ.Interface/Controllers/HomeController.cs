using MediatR;
using MeetIQ.Application.Features.Dashboard.Queries.GetDashboardQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace MeetIQ.Interface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard");
            return View();
        }
        

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var fullName = User.FindFirst("FullName")?.Value ?? "User";

            var result = await mediator.Send(new GetDashboardQuery
            {
                UserId = userId,
                UserFullName = fullName
            });

            ViewData["Title"] = "Dashboard";
            return View(result);
        }

        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ViewData["RequestId"] = requestId;
            return View("Error");
        }
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
