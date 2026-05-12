using MediatR;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Summaries.Queries.GetSummaryByIdQuery
{
    public class GetSummaryByIdQueryHandler
        : IRequestHandler<GetSummaryByIdQuery, SummaryDetailsDto?>
    {
        private readonly ISummaryRepository repository;

        public GetSummaryByIdQueryHandler(ISummaryRepository repository)
        {
            this.repository = repository;
        }

        public async Task<SummaryDetailsDto?> Handle(
            GetSummaryByIdQuery request,
            CancellationToken ct)
        {
            return await repository.GetByIdAsync(request.SummaryId);
        }
    }
}