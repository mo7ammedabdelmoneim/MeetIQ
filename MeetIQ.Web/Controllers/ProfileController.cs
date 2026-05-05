using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeetIQ.Application.Features.Profile.Commands.ChangePasswordCommand;
using MeetIQ.Application.Features.Profile.Commands.UpdateProfileCommand;
using MeetIQ.Application.Features.Profile.Queries.GetMyProfileQuery;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Web.ViewModels.Profile;
using Microsoft.AspNetCore.Identity;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IMediator mediator;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileController(
            IMediator mediator,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.mediator = mediator;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        private string CurrentUserId =>
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "My Profile";

            var profile = await mediator.Send(new GetMyProfileQuery { UserId = CurrentUserId });

            if (profile == null) return NotFound();

            ViewBag.UpdateModel = new UpdateProfileViewModel
            {
                FullName = profile.FullName,
                Email = profile.Email,
                CurrentAvatarUrl = profile.AvatarUrl
            };

            ViewBag.PasswordModel = new ChangePasswordViewModel();

            return View(profile);
        }

         
        [HttpPost("Profile/UpdateProfile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var profile = await mediator.Send(new GetMyProfileQuery { UserId = CurrentUserId });
                ViewBag.UpdateModel = model;
                ViewBag.PasswordModel = new ChangePasswordViewModel();
                ViewBag.ActiveTab = "profile";
                return View("Index", profile);
            }

            await mediator.Send(new UpdateProfileCommand
            {
                UserId = CurrentUserId,
                FullName = model.FullName,
                AvatarFile = model.AvatarFile,
                RemoveAvatar = model.RemoveAvatar
            });

            // refresh the auth cookie so sidebar shows new name/avatar
            var user = await userManager.FindByIdAsync(CurrentUserId);
            await signInManager.RefreshSignInAsync(user);

            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost("Profile/ChangePassword"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var profile = await mediator.Send(new GetMyProfileQuery { UserId = CurrentUserId });
                ViewBag.UpdateModel = new UpdateProfileViewModel { FullName = profile!.FullName, Email = profile.Email, CurrentAvatarUrl = profile.AvatarUrl };
                ViewBag.PasswordModel = model;
                ViewBag.ActiveTab = "password";
                return View("Index", profile);
            }

            try
            {
                await mediator.Send(new ChangePasswordCommand
                {
                    UserId = CurrentUserId,
                    CurrentPassword = model.CurrentPassword,
                    NewPassword = model.NewPassword,
                    ConfirmNewPassword = model.ConfirmNewPassword
                });

                TempData["Success"] = "Password changed successfully.";
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var profile = await mediator.Send(new GetMyProfileQuery { UserId = CurrentUserId });
                ViewBag.UpdateModel = new UpdateProfileViewModel { FullName = profile!.FullName, Email = profile.Email, CurrentAvatarUrl = profile.AvatarUrl };
                ViewBag.PasswordModel = model;
                ViewBag.ActiveTab = "password";
                return View("Index", profile);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}