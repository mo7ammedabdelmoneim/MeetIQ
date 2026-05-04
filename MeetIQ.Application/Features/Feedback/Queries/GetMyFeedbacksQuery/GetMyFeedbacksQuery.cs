using MediatR;
using MeetIQ.Application.Common;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Feedback.DTOs;

namespace MeetIQ.Application.Features.Feedback.Queries.GetMyFeedbacksQuery
{
    public class GetMyFeedbacksQuery : IRequest<PagedResult<FeedbackListItemDto>>
    {
        public string UserId { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}