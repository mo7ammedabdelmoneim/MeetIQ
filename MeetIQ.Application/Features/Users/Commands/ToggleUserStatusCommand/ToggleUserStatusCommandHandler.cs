using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;

namespace MeetIQ.Application.Features.Users.Commands.ToggleUserStatusCommand
{
    public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand, bool>
    {
        private readonly IUserRepository userRepository;

        public ToggleUserStatusCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(
            ToggleUserStatusCommand request,
            CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(x => x.Id == request.UserId);

            if (user == null)
                throw new NotFoundException("User not found");

            user.IsActive = !user.IsActive;

            userRepository.Update(user);
            await userRepository.SaveChangesAsync();

            return user.IsActive;
        }
    }
}