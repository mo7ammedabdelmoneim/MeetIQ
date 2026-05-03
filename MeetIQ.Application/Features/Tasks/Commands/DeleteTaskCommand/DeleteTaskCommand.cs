using MediatR;
using MeetIQ.Application.Interfaces;

namespace MeetIQ.Application.Features.Tasks.Commands.DeleteTaskCommand
{
    public class DeleteTaskCommand : ICommand<Unit>
    {
        public Guid TaskId { get; set; }
    }
}
