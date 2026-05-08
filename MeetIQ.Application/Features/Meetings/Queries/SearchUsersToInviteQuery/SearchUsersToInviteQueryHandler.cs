using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Meetings.Queries.SearchUsersToInviteQuery
{
    public class SearchUsersToInviteQueryHandler
        : IRequestHandler<SearchUsersToInviteQuery, List<UserSearchResultDto>>
    {
        private readonly IMeetingRepository meetingRepository;

        public SearchUsersToInviteQueryHandler(IMeetingRepository meetingRepository)
        {
            this.meetingRepository = meetingRepository;
        }

        public async Task<List<UserSearchResultDto>> Handle(
            SearchUsersToInviteQuery request,
            CancellationToken cancellationToken)
        {
            return await meetingRepository.SearchUsersToInviteAsync(
                request.Term,
                request.MeetingId,
                request.HostId,
                request.Limit);
        }
    }
}