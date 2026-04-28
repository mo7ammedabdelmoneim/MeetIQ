using MeetIQ.Application.Features.Auth.Commands.LoginCommand;
using MeetIQ.Application.Features.Auth.Commands.RegisterCommand;
using MeetIQ.Application.Features.Auth.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Interfaces.Services
{
    public interface IAuthService 
    {
        Task<AuthResponse> RegisterAsync(RegisterDto dto);
        Task<AuthResponse> LoginAsync(LoginDto dto);
        Task<AuthResponse> ExternalLoginAsync(ExternalLoginInfo info);
    }
}
