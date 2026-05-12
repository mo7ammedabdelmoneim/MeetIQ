using MediatR;

namespace MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptByIdQuery
{
    public class GetTranscriptByIdQuery : IRequest<TranscriptDetailsDto?>
    {
        public Guid TranscriptId { get; set; }
    }

    public class TranscriptDetailsDto
    {
        public Guid Id { get; set; }
        public Guid MeetingId { get; set; }
        public string MeetingTitle { get; set; } = string.Empty;
        public string HostId { get; set; } = string.Empty;
        public string? Text { get; set; }
        public string? Language { get; set; }
        public string? AudioFilePath { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> ParticipantIds { get; set; } = [];
    }
}