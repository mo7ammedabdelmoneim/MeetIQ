using MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptByIdQuery;
using MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptsQuery;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface ITranscriptRepository:IRepository<MeetingTranscript>
    {
        Task<TranscriptDetailsDto?> GetByIdAsync(Guid transcriptId);

        Task<List<TranscriptListItemDto>> GetUserTranscriptsAsync(string userId);
    }
}