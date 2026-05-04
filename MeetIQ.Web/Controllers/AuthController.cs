using MediatR;
using MeetIQ.Application.Features.Auth.Commands.ExternalLoginCommand;
using MeetIQ.Application.Features.Auth.Commands.LoginCommand;
using MeetIQ.Application.Features.Auth.Commands.RegisterCommand;
using MeetIQ.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeetIQ.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser> signInManager;

        public AuthController(IMediator mediator, SignInManager<ApplicationUser> signInManager)
        {
            _mediator = mediator;
            this.signInManager = signInManager;
        }


        //  Register 
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var command = new RegisterCommand(dto);
                var result = await _mediator.Send(command);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }



        
        //  Login 
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var command = new LoginCommand(dto);
                var result = await _mediator.Send(command);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }



        // Logout
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        //ExternalLogin
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error from provider: {remoteError}");
                return RedirectToAction("Login");
            }

            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ModelState.AddModelError("", "Error loading external login info");
                return RedirectToAction("Login");
            }

            try
            {
                var command = new ExternalLoginCommand(info, returnUrl);
                var result = await _mediator.Send(command);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Login");
            }
        }
    }
}
