using MeetIQ.Application.Features.Tasks.DTOs;
using MeetIQ.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MeetIQ.Interface.ViewModels.Tasks
{
    public class CreateTaskViewModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public TaskPriority Priority { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
        public Guid? MeetingId { get; set; }

        public List<MeetingSelectDto> Meetings { get; set; } = new();

    }
}
