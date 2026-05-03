using FluentValidation;

namespace MeetIQ.Application.Features.Auth.Commands.LoginCommand
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.LoginDto)
                .SetValidator(new LoginDtoValidator());
        }
    }
}
