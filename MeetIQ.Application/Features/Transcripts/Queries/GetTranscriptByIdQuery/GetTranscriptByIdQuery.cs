using MediatR;

namespace MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptByIdQuery
{
    public class GetTranscriptByIdQuery : IRequest<TranscriptDetailsDto?>
    {
        public Guid TranscriptId { get; set; }
    }
}