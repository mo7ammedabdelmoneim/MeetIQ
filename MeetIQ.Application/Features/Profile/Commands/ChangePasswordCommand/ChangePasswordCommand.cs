using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Profile.Commands.ChangePasswordCommand
{
    public class ChangePasswordCommand : ICommand<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}