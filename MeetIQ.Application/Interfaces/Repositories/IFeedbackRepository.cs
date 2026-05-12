using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Feedback.DTOs;
using MeetIQ.Application.Features.Feedback.Queries.GetFeedbacksQuery;
using MeetIQ.Application.Features.Feedback.Queries.GetMyFeedbacksQuery;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface IFeedbackRepository : IRepository<FeedbackReport>
    {
        Task<FeedbackDetailsDto?> GetByIdAsync(Guid id);
        Task<PagedResult<FeedbackListItemDto>> GetFeedbacksAsync(GetFeedbacksQuery query);
        Task<PagedResult<FeedbackListItemDto>> GetMyFeedbacksAsync(GetMyFeedbacksQuery query);
        Task<int> GetOpenFeedbacksCount();
    }
}