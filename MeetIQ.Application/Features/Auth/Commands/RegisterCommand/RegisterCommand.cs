using MeetIQ.Application.Features.Auth.DTOs;
using MeetIQ.Application.Interfaces;
namespace MeetIQ.Application.Features.Auth.Commands.RegisterCommand
{

    public class RegisterCommand : ICommand<AuthResponse>
    {
        public RegisterDto RegisterDto { get; set; }

        public RegisterCommand(RegisterDto registerDto)
        {
            RegisterDto = registerDto;
        }
    }
}
