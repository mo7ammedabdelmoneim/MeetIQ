using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Application.Features.Meetings.Queries.GetMeetingByIdQuery
{
    public class GetMeetingByIdQueryHandler : IRequestHandler<GetMeetingByIdQuery, Meeting>
    {
        private readonly IMeetingRepository meetingRepository;

        public GetMeetingByIdQueryHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
        }

        public async Task<Meeting> Handle(GetMeetingByIdQuery request, CancellationToken cancellationToken)
        {
            return await meetingRepository.GetAsync(x => x.Id == request.Id);
        }
    }
}
