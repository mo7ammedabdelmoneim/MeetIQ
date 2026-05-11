using MeetIQ.Application.Features.Tasks.DTOs;
using MeetIQ.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MeetIQ.Web.ViewModels.Tasks
{
    public class UpdateTaskViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TaskPriority Priority { get; set; }

        public Domain.Enums.TaskStatus Status { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid? MeetingId { get; set; }
        public List<MeetingSelectDto> Meetings { get; set; } = new();

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Assign To")]
        public string? AssigneeEmail { get; set; }

        public bool ClearAssignee { get; set; }
    }
}