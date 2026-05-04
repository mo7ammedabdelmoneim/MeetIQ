using MediatR;
using MeetIQ.Application.Features.Feedback.DTOs;

namespace MeetIQ.Application.Features.Feedback.Queries.GetFeedbackByIdQuery
{
    public class GetFeedbackByIdQuery : IRequest<FeedbackDetailsDto?>
    {
        public Guid Id { get; set; }
    }
}