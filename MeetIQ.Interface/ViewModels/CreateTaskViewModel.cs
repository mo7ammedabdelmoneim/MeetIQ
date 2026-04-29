using MeetIQ.Application.Features.Tasks.DTOs;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Interface.ViewModels
{
    public class CreateTaskViewModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }

        public Guid? MeetingId { get; set; }

        public List<MeetingSelectDto> Meetings { get; set; } = new();

    }
}
