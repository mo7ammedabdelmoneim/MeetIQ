using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;

namespace MeetIQ.Application.Features.Meetings.Queries.GetMeetingByIdQuery
{
    public class GetMeetingByIdQuery : IRequest<MeetingDetailsDto?>
    {
        public Guid MeetingId { get; set; }
    }
}