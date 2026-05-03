using MediatR;
using MeetIQ.Application.Features.Auth.DTOs;
using MeetIQ.Application.Interfaces.Services;

namespace MeetIQ.Application.Features.Auth.Commands.RegisterCommand
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IAuthService _authService;

        public RegisterHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterAsync(request.RegisterDto);
        }
    }
}
