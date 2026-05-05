using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Services;

namespace MeetIQ.Application.Features.Profile.Commands.ChangePasswordCommand
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IAuthService authService;

        public ChangePasswordCommandHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<bool> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            var user = await authService.GetByIdAsync(request.UserId);

            if (user == null)
                throw new NotFoundException("User not found");

            var (succeeded, errors) = await authService.ChangePasswordAsync(
                                                                            request.UserId,
                                                                            request.CurrentPassword,
                                                                            request.NewPassword);

            if (!succeeded)
                throw new BadRequestException(string.Join(", ", errors));

            return true;
        }
    }
}