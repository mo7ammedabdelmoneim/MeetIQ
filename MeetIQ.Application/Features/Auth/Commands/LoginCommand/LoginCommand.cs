using MediatR;
using MeetIQ.Application.Features.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Auth.Commands.LoginCommand
{
    public class LoginCommand : IRequest<AuthResponse>
    {
        public LoginDto LoginDto { get; set; }

        public LoginCommand(LoginDto dto)
        {
            LoginDto = dto;
        }
    }
}
