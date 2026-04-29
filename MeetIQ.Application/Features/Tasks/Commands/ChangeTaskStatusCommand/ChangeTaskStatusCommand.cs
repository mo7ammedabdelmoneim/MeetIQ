using MediatR;
using MeetIQ.Application.Interfaces;


namespace MeetIQ.Application.Features.Tasks.Commands.ChangeTaskStatusCommand
{
    public class ChangeTaskStatusCommand : ICommand<Unit>
    {
        public Guid TaskId { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
    }
}
