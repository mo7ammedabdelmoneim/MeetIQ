using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery
{
    public class GetUserMeetingsQueryHandler : IRequestHandler<GetUserMeetingsQuery, List<MeetingSelectDto>>
    {
        private readonly IMeetingRepository meetingRepository;

        public GetUserMeetingsQueryHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
        }

        public async Task<List<MeetingSelectDto>> Handle(
            GetUserMeetingsQuery request,
            CancellationToken cancellationToken)
        {
            return await meetingRepository.GetUserMeetingSelectListAsync(request.UserId);
        }
    }
}