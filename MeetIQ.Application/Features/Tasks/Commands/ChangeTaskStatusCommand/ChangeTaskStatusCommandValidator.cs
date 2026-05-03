using FluentValidation;

namespace MeetIQ.Application.Features.Tasks.Commands.ChangeTaskStatusCommand
{
    public class ChangeTaskStatusCommandValidator : AbstractValidator<ChangeTaskStatusCommand>
    {
        public ChangeTaskStatusCommandValidator()
        {
            RuleFor(x => x.TaskId)
                .NotEmpty();

            RuleFor(x => x.Status)
                .IsInEnum();
        }
    }
}
