namespace MeetIQ.Domain.Entities
{
    public class MeetingTranscript
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RawText { get; set; } = string.Empty;   // full Whisper output
        public string? AudioFileUrl { get; set; }                    // encrypted at rest
        public bool IsProcessed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; } = null!;
    }

   
}
