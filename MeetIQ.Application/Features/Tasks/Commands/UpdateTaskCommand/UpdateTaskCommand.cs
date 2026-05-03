using MediatR;
using MeetIQ.Application.Interfaces;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Tasks.Commands.UpdateTaskCommand
{
    public class UpdateTaskCommand : ICommand<Unit>
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }

        public TaskPriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? MeetingId { get; set; }


    }
}
