using MediatR;
using MeetIQ.Application.Common;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Users.DTOs;

namespace MeetIQ.Application.Features.Users.Queries.GetUsersQuery
{
    public class GetUsersQuery : IRequest<PagedResult<UserListItemDto>>
    {
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
        public string? Role { get; set; }        
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}