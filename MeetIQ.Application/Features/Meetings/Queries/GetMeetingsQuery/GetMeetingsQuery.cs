using MediatR;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Queries.GetMeetingsQuery
{
    public class GetMeetingsQuery : IRequest<PagedResult<MeetingListItemDto>>
    {
        public string UserId { get; set; } = string.Empty;
        public MeetingStatus? Status { get; set; }
        public string? Search { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}