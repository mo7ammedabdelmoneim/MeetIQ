using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Users.DTOs;
using MeetIQ.Application.Features.Users.Queries.GetUsersQuery;
using MeetIQ.Domain.Entities;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<UserDetailsDto?> GetUserByIdAsync(string userId);
        Task<PagedResult<UserListItemDto>> GetUsersAsync(GetUsersQuery query);

    }
}
