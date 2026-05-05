using MediatR;
using MeetIQ.Application.Features.Profile.DTOs;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Profile.Queries.GetMyProfileQuery
{
    public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, MyProfileDto?>
    {
        private readonly IUserRepository userRepository;

        public GetMyProfileQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<MyProfileDto?> Handle(
            GetMyProfileQuery request,
            CancellationToken cancellationToken)
        {
            return await userRepository.GetMyProfileAsync(request.UserId);
        }
    }
}