using MediatR;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Features.Meetings.Queries.GetMeetingByIdQuery
{
    public class GetMeetingByIdQuery : IRequest<Meeting>
    {
        public Guid Id { get; set; }
    }
}
