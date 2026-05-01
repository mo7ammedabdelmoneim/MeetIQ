using MediatR;
using MeetIQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Meetings.Queries.GetMeetingByIdQuery
{
    public class GetMeetingByIdQuery : IRequest<Meeting>
    {
        public Guid Id { get; set; }
    }
}
