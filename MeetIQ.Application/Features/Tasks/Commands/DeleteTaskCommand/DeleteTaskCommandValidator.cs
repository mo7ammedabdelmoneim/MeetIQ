using FluentValidation;

namespace MeetIQ.Application.Features.Tasks.Commands.DeleteTaskCommand
{
    public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(x => x.TaskId)
                .NotEmpty();
        }
    }
}
