using MediatR;
using MeetIQ.Application.Interfaces;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Tasks.Commands.UpdateTaskCommand
{
    //public class UpdateTaskCommand : ICommand<Unit>
    //{
    //    public Guid Id { get; set; }

    //    public string? Title { get; set; }
    //    public string? Description { get; set; }

    //    public TaskPriority? Priority { get; set; }
    //    public DateTime? DueDate { get; set; }
    //    public Guid? MeetingId { get; set; }

    //    public string? AssigneeEmail { get; set; }
    //}

    public class UpdateTaskCommand : ICommand<bool>
    {
        public Guid TaskId { get; set; }
        public string RequesterId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string? AssigneeEmail { get; set; }
        public bool ClearAssignee { get; set; }
    }

}
