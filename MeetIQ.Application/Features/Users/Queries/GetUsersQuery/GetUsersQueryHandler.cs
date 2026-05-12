using MediatR;
using MeetIQ.Application.Common.Results;
using MeetIQ.Application.Features.Users.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Users.Queries.GetUsersQuery
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResult<UserListItemDto>>
    {
        private readonly IUserRepository userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<PagedResult<UserListItemDto>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken)
        {
            return await userRepository.GetUsersAsync(request);
        }
    }
}