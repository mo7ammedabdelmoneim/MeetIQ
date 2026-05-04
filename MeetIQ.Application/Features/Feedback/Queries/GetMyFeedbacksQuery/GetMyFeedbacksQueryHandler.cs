using MediatR;
using MeetIQ.Application.Common;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Feedback.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Feedback.Queries.GetMyFeedbacksQuery
{
    public class GetMyFeedbacksQueryHandler : IRequestHandler<GetMyFeedbacksQuery, PagedResult<FeedbackListItemDto>>
    {
        private readonly IFeedbackRepository feedbackRepository;

        public GetMyFeedbacksQueryHandler(IFeedbackRepository feedbackRepository)
        {
            this.feedbackRepository = feedbackRepository;
        }

        public async Task<PagedResult<FeedbackListItemDto>> Handle(GetMyFeedbacksQuery request, CancellationToken cancellationToken)
        {
            return await feedbackRepository.GetMyFeedbacksAsync(request);
        }
    }
}