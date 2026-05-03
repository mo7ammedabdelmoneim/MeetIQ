using FluentValidation;

namespace MeetIQ.Application.Features.Tasks.Commands.UpdateTaskCommand
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Title)
                .MinimumLength(3)
                .MaximumLength(200)
                .When(x => x.Title != null);

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .When(x => x.Description != null);

            RuleFor(x => x.DueDate)
                .Must(date => date!.Value.Date >= DateTime.UtcNow.Date)
                .When(x => x.DueDate.HasValue)
                .WithMessage("DueDate must be today or in the future");

            RuleFor(x => x.Priority)
                .IsInEnum()
                .When(x => x.Priority.HasValue);
        }
    }
}
