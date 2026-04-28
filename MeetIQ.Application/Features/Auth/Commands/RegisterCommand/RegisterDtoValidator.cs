using FluentValidation;

namespace MeetIQ.Application.Features.Auth.Commands.RegisterCommand
{

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
             .NotEmpty()
             .MinimumLength(8)
             .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
             .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
             .Matches("[0-9]").WithMessage("Password must contain at least one number")
             .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
        }
    }
}
