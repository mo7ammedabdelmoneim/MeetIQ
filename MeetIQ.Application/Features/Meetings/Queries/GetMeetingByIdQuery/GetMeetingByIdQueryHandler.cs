using MediatR;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Domain.Entities;

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
