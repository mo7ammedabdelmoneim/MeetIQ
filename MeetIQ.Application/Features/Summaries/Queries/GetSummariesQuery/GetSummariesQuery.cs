using MediatR;

namespace MeetIQ.Application.Features.Summaries.Queries.GetSummariesQuery
{
    public class GetSummariesQuery : IRequest<List<SummaryListItemDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class SummaryListItemDto
    {
        public Guid Id { get; set; }
        public Guid MeetingId { get; set; }
        public string MeetingTitle { get; set; } = string.Empty;
        public string SummaryText { get; set; } = string.Empty;
        public bool IsEdited { get; set; }
        public DateTime GeneratedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}