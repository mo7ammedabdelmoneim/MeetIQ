using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Meetings.Queries.GetUserMeetingsQuery
{
    public class GetUserMeetingsQueryHandler : IRequestHandler<GetUserMeetingsQuery, List<MeetingListItemDto>>
    {
        private readonly IMeetingRepository _repo;

        public GetUserMeetingsQueryHandler(IMeetingRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<MeetingListItemDto>> Handle(GetUserMeetingsQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetUserMeetingsAsync(request);
        }
    }
}
