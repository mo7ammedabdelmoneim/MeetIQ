using Microsoft.AspNetCore.Identity;

namespace MeetIQ.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public ICollection<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();
        public ICollection<MeetingParticipant> Participations { get; set; } = new List<MeetingParticipant>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public ICollection<FeedbackReport> FeedbackReports { get; set; } = new List<FeedbackReport>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();           
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();   
    }
}
