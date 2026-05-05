using MediatR;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Meetings.Queries.GetMeetingsQuery
{
    public class GetMeetingsQueryHandler : IRequestHandler<GetMeetingsQuery, PagedResult<MeetingListItemDto>>
    {
        private readonly IMeetingRepository meetingRepository;

        public GetMeetingsQueryHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
        }

        public async Task<PagedResult<MeetingListItemDto>> Handle(
            GetMeetingsQuery request,
            CancellationToken cancellationToken)
        {
            return await meetingRepository.GetMeetingsAsync(request);
        }
    }
}
