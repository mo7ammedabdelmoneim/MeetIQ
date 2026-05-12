using MediatR;

namespace MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptsQuery
{
    public class GetTranscriptsQuery : IRequest<List<TranscriptListItemDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}