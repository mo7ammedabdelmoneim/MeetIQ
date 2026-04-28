using FluentValidation;

namespace MeetIQ.Application.Features.Auth.Commands.RegisterCommand
{

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.RegisterDto)
                .SetValidator(new RegisterDtoValidator());
        }
    }
}