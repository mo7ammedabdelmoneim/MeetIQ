using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;

namespace MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery
{
    public class GetUserMeetingsQuery : IRequest<List<MeetingListItemDto>>
    {
        public string UserId { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
