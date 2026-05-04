using FluentValidation;

namespace MeetIQ.Application.Features.Users.Commands.ToggleUserStatusCommand
{
    public class ToggleUserStatusCommandValidator : AbstractValidator<ToggleUserStatusCommand>
    {
        public ToggleUserStatusCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.AdminId)
                .NotEmpty().WithMessage("AdminId is required");

            RuleFor(x => x)
                .Must(x => x.UserId != x.AdminId)
                .WithMessage("You cannot deactivate your own account");
        }
    }
}