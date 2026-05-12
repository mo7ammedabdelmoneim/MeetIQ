using MeetIQ.Application.Features.Summaries.Queries.GetSummariesQuery;
using MeetIQ.Application.Features.Summaries.Queries.GetSummaryByIdQuery;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface ISummaryRepository:IRepository<MeetingSummary>
    {
        Task<SummaryDetailsDto?> GetByIdAsync(Guid summaryId);

        Task<List<SummaryListItemDto>> GetUserSummariesAsync(string userId);
    }
}