using MeetIQ.Application.Features.Auth.DTOs;
using MeetIQ.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MeetIQ.Application.Features.Auth.Commands.ExternalLoginCommand
{
    public class ExternalLoginCommand : ICommand<AuthResponse>
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
