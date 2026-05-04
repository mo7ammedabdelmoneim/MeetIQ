using MeetIQ.Application.Interfaces;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Feedback.Commands.UpdateFeedbackStatusCommand
{
    public class UpdateFeedbackStatusCommand : ICommand<bool>
    {
        public Guid FeedbackId { get; set; }
        public FeedbackStatus NewStatus { get; set; }
    }
}