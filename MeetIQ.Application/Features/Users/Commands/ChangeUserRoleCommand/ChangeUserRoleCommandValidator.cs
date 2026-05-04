using FluentValidation;
using MeetIQ.Application.Common.Constants;

namespace MeetIQ.Application.Features.Users.Commands.ChangeUserRoleCommand
{
    public class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
    {
        private static readonly string[] AllowedRoles = [Roles.Admin, Roles.User];

        public ChangeUserRoleCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.NewRole)
                .NotEmpty()
                .Must(r => AllowedRoles.Contains(r))
                .WithMessage("Role must be Admin or User");

            RuleFor(x => x)
                .Must(x => x.UserId != x.AdminId)
                .WithMessage("You cannot change your own role");
        }
    }
}