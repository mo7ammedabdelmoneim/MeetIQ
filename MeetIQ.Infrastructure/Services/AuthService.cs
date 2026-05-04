using MeetIQ.Application.Common.Constants;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Features.Auth.Commands.LoginCommand;
using MeetIQ.Application.Features.Auth.Commands.RegisterCommand;
using MeetIQ.Application.Features.Auth.DTOs;
using MeetIQ.Application.Interfaces.Services;
using MeetIQ.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace MeetIQ.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new BadRequestException("Email already exists");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            if (await roleManager.RoleExistsAsync(Roles.User))
            {
                await _userManager.AddToRoleAsync(user, Roles.User);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("FullName", user.FullName ?? ""),
                    new Claim("ProfileImage", user.AvatarUrl ?? "/images/users/default-user.png"),
                    new Claim(ClaimTypes.Email, user.Email)
                };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = roles.ToList()
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginDto dto)
        {

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new BadRequestException("Invalid email or password");

            var isValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isValid)
                throw new BadRequestException("Invalid email or password");

            if(!user.IsActive)
                throw new BadRequestException("Inactive email");


            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim("FullName", user.FullName ?? ""),
                        new Claim("ProfileImage", user.AvatarUrl ?? "/images/users/default-user.png"),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            await _signInManager.SignInWithClaimsAsync(
                user,
                isPersistent: dto.RememberMe,
                claims
            );

            return new AuthResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles.ToList(),
            };
        }

        public async Task<AuthResponse> ExternalLoginAsync(ExternalLoginInfo info)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true
            );

            if (signInResult.Succeeded)
            {
                var existingUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                if (existingUser == null)
                    throw new NotFoundException("User not found");

                await _signInManager.SignOutAsync();

                var existingUserRoles = await _userManager.GetRolesAsync(existingUser);

                var existingUserClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, existingUser.Id),
                        new Claim("FullName", existingUser.FullName ?? ""),
                        new Claim("ProfileImage", existingUser.AvatarUrl ?? "/images/users/default-user.png"),
                        new Claim(ClaimTypes.Email, existingUser.Email)
                    };

                foreach (var role in existingUserRoles)
                    existingUserClaims.Add(new Claim(ClaimTypes.Role, role));

                await _signInManager.SignInWithClaimsAsync(existingUser, false, existingUserClaims);

                return new AuthResponse
                {
                    UserId = existingUser.Id,
                    Email = existingUser.Email,
                    FullName = existingUser.FullName,
                    Roles = existingUserRoles.ToList()
                };
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
                throw new BadRequestException("Email not provided by external provider");

            var name = info.Principal.FindFirstValue(ClaimTypes.Name);

            var picture = info.Principal.FindFirst("picture")?.Value;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    FullName = name,
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    AvatarUrl = picture ?? "/images/users/default-user.png"
                };

                var createResult = await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                    throw new BadRequestException(string.Join(", ", createResult.Errors.Select(e => e.Description)));

                if (await roleManager.RoleExistsAsync(Roles.User))
                {
                    await _userManager.AddToRoleAsync(user, Roles.User);
                }
            }

            var existingLogins = await _userManager.GetLoginsAsync(user);

            if (!existingLogins.Any(l => l.LoginProvider == info.LoginProvider))
            {
                var addLoginResult = await _userManager.AddLoginAsync(user, info);

                if (!addLoginResult.Succeeded)
                    throw new Exception(string.Join(", ", addLoginResult.Errors.Select(e => e.Description)));
            }
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("FullName", user.FullName ?? ""),
                    new Claim("ProfileImage", user.AvatarUrl ?? "/images/users/default-user.png"),
                    new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };
        }



        public async Task<ApplicationUser?> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task RemoveFromRolesAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task AddToRoleAsync(ApplicationUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }
    }
}
