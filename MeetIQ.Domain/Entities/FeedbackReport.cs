using MeetIQ.Domain.Enums;

namespace MeetIQ.Domain.Entities
{
    public class FeedbackReport
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public FeedbackType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public FeedbackStatus Status { get; set; } = FeedbackStatus.Open;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ReporterId { get; set; } = string.Empty;
        public ApplicationUser Reporter { get; set; } = null!;
    }

   
}
