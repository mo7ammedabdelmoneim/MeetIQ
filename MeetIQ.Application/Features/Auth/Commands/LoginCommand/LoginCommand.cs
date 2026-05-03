using MeetIQ.Application.Features.Auth.DTOs;
using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Auth.Commands.LoginCommand
{
    public class LoginCommand : ICommand<AuthResponse>
    {
        public LoginDto LoginDto { get; set; }

        public LoginCommand(LoginDto dto)
        {
            LoginDto = dto;
        }
    }
}
