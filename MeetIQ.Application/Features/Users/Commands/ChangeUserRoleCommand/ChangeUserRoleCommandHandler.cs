using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using MeetIQ.Domain.Entities;
using MeetIQ.Application.Interfaces.Services;

namespace MeetIQ.Application.Features.Users.Commands.ChangeUserRoleCommand
{
    public class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IAuthService authService;

        public ChangeUserRoleCommandHandler(
            IUserRepository userRepository,
            IAuthService authService)
        {
            this.userRepository = userRepository;
            this.authService = authService;
        }

        public async Task<bool> Handle(
            ChangeUserRoleCommand request,
            CancellationToken cancellationToken)
        {
            var user = await authService.GetByIdAsync(request.UserId);

            if (user == null)
                throw new NotFoundException("User not found");

            // Remove current roles
            var currentRoles = await authService.GetUserRolesAsync(user);
            await authService.RemoveFromRolesAsync(user, currentRoles);

            // Assign new role
            await authService.AddToRoleAsync(user, request.NewRole);

            return true;
        }
    }
}