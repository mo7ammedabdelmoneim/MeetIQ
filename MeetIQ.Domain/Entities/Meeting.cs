using MeetIQ.Domain.Enums;

namespace MeetIQ.Domain.Entities
{
    public class Meeting
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string JitsiRoomId { get; set; } = string.Empty;   
        public DateTime ScheduledAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public MeetingStatus Status { get; set; } = MeetingStatus.Scheduled;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string HostId { get; set; } = string.Empty;
        public ApplicationUser Host { get; set; } = null!;

        // Back-link to calendar entry
        public CalendarEvent? CalendarEvent { get; set; }

        // Navigation
        public MeetingTranscript? Transcript { get; set; }
        public MeetingSummary? Summary { get; set; }
        public ICollection<MeetingParticipant> Participants { get; set; } = new List<MeetingParticipant>();
        public ICollection<MeetingInvitation> Invitations { get; set; } = new List<MeetingInvitation>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }

   
}
