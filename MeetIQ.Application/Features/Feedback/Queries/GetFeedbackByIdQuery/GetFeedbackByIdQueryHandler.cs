using MediatR;
using MeetIQ.Application.Features.Feedback.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Feedback.Queries.GetFeedbackByIdQuery
{
    public class GetFeedbackByIdQueryHandler : IRequestHandler<GetFeedbackByIdQuery, FeedbackDetailsDto?>
    {
        private readonly IFeedbackRepository feedbackRepository;

        public GetFeedbackByIdQueryHandler(IFeedbackRepository feedbackRepository)
        {
            this.feedbackRepository = feedbackRepository;
        }

        public async Task<FeedbackDetailsDto?> Handle(GetFeedbackByIdQuery request, CancellationToken cancellationToken)
        {
            return await feedbackRepository.GetByIdAsync(request.Id);
        }
    }
}