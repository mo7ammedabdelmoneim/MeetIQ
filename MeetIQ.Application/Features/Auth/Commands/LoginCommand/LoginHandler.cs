using MediatR;
using MeetIQ.Application.Features.Auth.DTOs;
using MeetIQ.Application.Interfaces.Services;

namespace MeetIQ.Application.Features.Auth.Commands.LoginCommand
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IAuthService _authService;

        public LoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginAsync(request.LoginDto);
        }
    }
}
