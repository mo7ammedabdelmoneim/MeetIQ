using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Users.Commands.ToggleUserStatusCommand
{
    public class ToggleUserStatusCommand : ICommand<bool>
    {
        public string UserId { get; set; } = string.Empty;
        public string AdminId { get; set; } = string.Empty; 
    }
}