using MediatR;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Feedback.DTOs;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Feedback.Queries.GetFeedbacksQuery
{
    public class GetFeedbacksQuery : IRequest<PagedResult<FeedbackListItemDto>>
    {
        public FeedbackStatus? Status { get; set; }
        public FeedbackType? Type { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}