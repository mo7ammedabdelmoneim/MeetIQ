using MeetIQ.Domain.Enums;

namespace MeetIQ.Domain.Entities
{
    public class MeetingTranscript
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; } = null!;

        public string? AudioFilePath { get; set; }

        public string? Text { get; set; }
        public string? Language { get; set; }

        public TranscriptStatus Status { get; set; } = TranscriptStatus.PendingTranscription;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
