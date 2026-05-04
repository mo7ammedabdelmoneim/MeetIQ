using MeetIQ.Application.Features.Tasks.DTOs;
using MeetIQ.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MeetIQ.Web.ViewModels.Tasks
{
    public class UpdateTaskViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public TaskPriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? MeetingId { get; set; }

        public List<MeetingSelectDto> Meetings { get; set; } = new();
    }
}
