using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeetIQ.Application.Features.Users.Commands.ChangeUserRoleCommand;
using MeetIQ.Application.Features.Users.Commands.ToggleUserStatusCommand;
using MeetIQ.Application.Features.Users.Queries.GetUserByIdQuery;
using MeetIQ.Application.Features.Users.Queries.GetUsersQuery;
using MeetIQ.Web.ViewModels.Admin.Users;
using MeetIQ.Application.Common.Constants;

namespace MeetIQ.Web.Controllers.Admin
{
    [Authorize(Roles = Roles.Admin)]
    [Route("Admin/Users")]
    public class AdminUsersController : Controller
    {
        private readonly IMediator mediator;

        public AdminUsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET /Admin/Users
        [HttpGet("")]
        public async Task<IActionResult> Index(
            string? search,
            bool? isActive,
            string? role,
            int page = 1)
        {
            ViewData["Title"] = "Users";

            var result = await mediator.Send(new GetUsersQuery
            {
                Search = search,
                IsActive = isActive,
                Role = role,
                Page = page
            });

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentIsActive = isActive;
            ViewBag.CurrentRole = role;

            return View("/Views/Admin/Users/Index.cshtml",result);
        }

        // GET /Admin/Users/Details/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            ViewData["Title"] = "User Details";

            var user = await mediator.Send(new GetUserByIdQuery { UserId = id });

            if (user == null)
                return NotFound();

            return View("/Views/Admin/Users/Details.cshtml",user);
        }

        // POST /Admin/Users/ToggleStatus
        [HttpPost("ToggleStatus"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string userId)
        {
            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            await mediator.Send(new ToggleUserStatusCommand
            {
                UserId = userId,
                AdminId = adminId
            });

            TempData["Success"] = "User status updated successfully.";
            return RedirectToAction(nameof(Details), new { id = userId });
        }

        // POST /Admin/Users/ChangeRole
        [HttpPost("ChangeRole"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(ChangeUserRoleViewModel model)
        {
            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            await mediator.Send(new ChangeUserRoleCommand
            {
                UserId = model.UserId,
                NewRole = model.NewRole,
                AdminId = adminId
            });

            TempData["Success"] = "User role updated successfully.";
            return RedirectToAction(nameof(Details), new { id = model.UserId });
        }
    }
}