using MeetIQ.Application.Features.Auth.Commands.LoginCommand;
using MeetIQ.Application.Features.Auth.Commands.RegisterCommand;
using MeetIQ.Application.Features.Auth.DTOs;
using MeetIQ.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MeetIQ.Application.Interfaces.Services
{
    public interface IAuthService 
    {
        Task<AuthResponse> RegisterAsync(RegisterDto dto);
        Task<AuthResponse> LoginAsync(LoginDto dto);
        Task<AuthResponse> ExternalLoginAsync(ExternalLoginInfo info);

        Task<ApplicationUser?> GetByIdAsync(string userId);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task RemoveFromRolesAsync(ApplicationUser user, IEnumerable<string> roles);
        Task AddToRoleAsync(ApplicationUser user, string role);
    }
}
