using MediatR;
using MeetIQ.Application.Common;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Feedback.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Feedback.Queries.GetFeedbacksQuery
{
    public class GetFeedbacksQueryHandler : IRequestHandler<GetFeedbacksQuery, PagedResult<FeedbackListItemDto>>
    {
        private readonly IFeedbackRepository feedbackRepository;

        public GetFeedbacksQueryHandler(IFeedbackRepository feedbackRepository)
        {
            this.feedbackRepository = feedbackRepository;
        }

        public async Task<PagedResult<FeedbackListItemDto>> Handle(GetFeedbacksQuery request, CancellationToken cancellationToken)
        {
            return await feedbackRepository.GetFeedbacksAsync(request);
        }
    }
}