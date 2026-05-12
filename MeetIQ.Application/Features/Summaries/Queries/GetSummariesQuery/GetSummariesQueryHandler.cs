using MediatR;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Summaries.Queries.GetSummariesQuery
{
    public class GetSummariesQueryHandler
        : IRequestHandler<GetSummariesQuery, List<SummaryListItemDto>>
    {
        private readonly ISummaryRepository repository;

        public GetSummariesQueryHandler(ISummaryRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<SummaryListItemDto>> Handle(
            GetSummariesQuery request,
            CancellationToken ct)
        {
            return await repository.GetUserSummariesAsync(request.UserId);
        }
    }
}