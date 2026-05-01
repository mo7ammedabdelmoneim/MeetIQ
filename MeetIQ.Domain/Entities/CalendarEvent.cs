namespace MeetIQ.Domain.Entities
{
    public class CalendarEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Color { get; set; } = "#3B82F6";   // color-coded type
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public string OwnerId { get; set; } = string.Empty;
        public ApplicationUser Owner { get; set; } = null!;

        public Guid? MeetingId { get; set; }
        public Meeting? Meeting { get; set; }

        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
