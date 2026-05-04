using MeetIQ.Application.Interfaces;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Feedback.Commands.CreateFeedbackCommand
{
    public class CreateFeedbackCommand : ICommand<Guid>
    {
        public FeedbackType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ReporterId { get; set; } = string.Empty;
    }
}