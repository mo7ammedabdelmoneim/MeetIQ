using MediatR;
using MeetIQ.Application.Features.Users.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Users.Queries.GetUserByIdQuery
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetailsDto?>
    {
        private readonly IUserRepository userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserDetailsDto?> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await userRepository.GetUserByIdAsync(request.UserId);
        }
    }
}