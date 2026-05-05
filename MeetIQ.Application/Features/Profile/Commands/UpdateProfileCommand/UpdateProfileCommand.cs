using MeetIQ.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MeetIQ.Application.Features.Profile.Commands.UpdateProfileCommand
{
    public class UpdateProfileCommand : ICommand<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public IFormFile? AvatarFile { get; set; }
        public bool RemoveAvatar { get; set; }
    }
}