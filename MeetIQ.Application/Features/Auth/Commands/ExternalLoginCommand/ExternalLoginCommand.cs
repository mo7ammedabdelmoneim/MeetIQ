using MediatR;
using MeetIQ.Application.Features.Auth.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Auth.Commands.ExternalLoginCommand
{
    public class ExternalLoginCommand : IRequest<AuthResponse>
    {
        public ExternalLoginInfo Info { get; set; }
        public string? ReturnUrl { get; set; }

        public ExternalLoginCommand(ExternalLoginInfo info, string? returnUrl)
        {
            Info = info;
            ReturnUrl = returnUrl;
        }
    }
}
