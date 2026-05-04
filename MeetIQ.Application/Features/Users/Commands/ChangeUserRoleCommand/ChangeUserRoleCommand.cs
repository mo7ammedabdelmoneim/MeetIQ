using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Users.Commands.ChangeUserRoleCommand
{
    public class ChangeUserRoleCommand : ICommand<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public string NewRole { get; set; } = string.Empty;  
        public string AdminId { get; set; } = string.Empty;
    }
}