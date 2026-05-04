using MeetIQ.Domain.Enums;

namespace MeetIQ.Web.ViewModels.Feedback
{
    public class UpdateFeedbackStatusViewModel
    {
        public Guid FeedbackId { get; set; }
        public FeedbackStatus NewStatus { get; set; }
    }
}