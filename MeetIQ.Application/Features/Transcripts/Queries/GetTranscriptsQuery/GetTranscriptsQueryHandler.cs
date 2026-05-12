using MediatR;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptsQuery
{
    public class GetTranscriptsQueryHandler
        : IRequestHandler<GetTranscriptsQuery, List<TranscriptListItemDto>>
    {
        private readonly ITranscriptRepository repository;

        public GetTranscriptsQueryHandler(ITranscriptRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<TranscriptListItemDto>> Handle(
            GetTranscriptsQuery request,
            CancellationToken ct)
        {
            return await repository.GetUserTranscriptsAsync(request.UserId);
        }
    }
}