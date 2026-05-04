using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Feedback.DTOs
{
    public class FeedbackDetailsDto
    {
        public Guid Id { get; set; }
        public FeedbackType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public FeedbackStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ReporterId { get; set; } = string.Empty;
        public string ReporterName { get; set; } = string.Empty;
        public string ReporterEmail { get; set; } = string.Empty;
    }
}