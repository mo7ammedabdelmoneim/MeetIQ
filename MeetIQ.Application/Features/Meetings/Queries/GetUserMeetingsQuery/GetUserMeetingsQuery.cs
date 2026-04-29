using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery
{
    public class GetUserMeetingsQuery : IRequest<List<MeetingListItemDto>>
    {
        public string UserId { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
