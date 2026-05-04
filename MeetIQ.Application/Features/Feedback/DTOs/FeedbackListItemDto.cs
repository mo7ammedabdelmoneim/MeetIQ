using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Feedback.DTOs
{
    public class FeedbackListItemDto
    {
        public Guid Id { get; set; }
        public FeedbackType Type { get; set; }       // ← enum, مش string
        public string Message { get; set; } = string.Empty;
        public FeedbackStatus Status { get; set; }   // ← enum, مش string
        public DateTime CreatedAt { get; set; }
        public string ReporterName { get; set; } = string.Empty;
        public string ReporterEmail { get; set; } = string.Empty;
    }
}