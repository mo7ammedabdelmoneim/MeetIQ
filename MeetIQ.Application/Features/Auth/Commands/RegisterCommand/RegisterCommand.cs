using MediatR;
using MeetIQ.Application.Features.Auth.DTOs;
namespace MeetIQ.Application.Features.Auth.Commands.RegisterCommand
{

    public class RegisterCommand : IRequest<AuthResponse>
    {
        public RegisterDto RegisterDto { get; set; }

        public RegisterCommand(RegisterDto registerDto)
        {
            RegisterDto = registerDto;
        }
    }
}
