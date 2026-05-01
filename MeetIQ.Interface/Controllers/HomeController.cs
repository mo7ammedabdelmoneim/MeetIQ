using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MeetIQ.Interface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Tasks");
            return View();
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
