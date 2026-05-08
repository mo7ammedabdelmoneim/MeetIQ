using MediatR;
using MeetIQ.Application.Features.Meetings.DTOs;

namespace MeetIQ.Application.Features.Meetings.Queries.SearchUsersToInviteQuery
{
    public class SearchUsersToInviteQuery : IRequest<List<UserSearchResultDto>>
    {
        public string Term { get; set; } = string.Empty;      
        public Guid MeetingId { get; set; }                   
        public string HostId { get; set; } = string.Empty;    
        public int Limit { get; set; } = 6;
    }
}