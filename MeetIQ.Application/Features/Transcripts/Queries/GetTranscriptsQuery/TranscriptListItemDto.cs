using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptsQuery
{
    public class TranscriptListItemDto
    {
        public Guid Id { get; set; }
        public Guid MeetingId { get; set; }
        public string MeetingTitle { get; set; } = string.Empty;
        public TranscriptStatus Status { get; set; }  
        public string? Language { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int WordCount => string.IsNullOrWhiteSpace(Text)
            ? 0
            : Text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
    }
}