using MediatR;
using MeetIQ.Application.Features.Auth.DTOs;
using MeetIQ.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Auth.Commands.ExternalLoginCommand
{
    public class ExternalLoginHandler : IRequestHandler<ExternalLoginCommand, AuthResponse>
    {
        private readonly IAuthService _authService;

        public ExternalLoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.ExternalLoginAsync(request.Info);
        }
    }
}
