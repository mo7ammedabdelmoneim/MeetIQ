using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;

namespace MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery
{
    public class GetUserMeetingsQuery : IRequest<List<MeetingSelectDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}