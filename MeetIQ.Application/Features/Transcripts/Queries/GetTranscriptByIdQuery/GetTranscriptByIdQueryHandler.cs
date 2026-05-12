using MediatR;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Transcripts.Queries.GetTranscriptByIdQuery
{
    public class GetTranscriptByIdQueryHandler
        : IRequestHandler<GetTranscriptByIdQuery, TranscriptDetailsDto?>
    {
        private readonly ITranscriptRepository repository;

        public GetTranscriptByIdQueryHandler(ITranscriptRepository repository)
        {
            this.repository = repository;
        }

        public async Task<TranscriptDetailsDto?> Handle(
            GetTranscriptByIdQuery request,
            CancellationToken ct)
        {
            return await repository.GetByIdAsync(request.TranscriptId);
        }
    }
}