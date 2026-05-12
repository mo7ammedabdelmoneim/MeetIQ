using MediatR;

namespace MeetIQ.Application.Features.Summaries.Queries.GetSummaryByIdQuery
{
    public class GetSummaryByIdQuery : IRequest<SummaryDetailsDto?>
    {
        public Guid SummaryId { get; set; }
    }
}