using FluentValidation;

namespace MeetIQ.Application.Features.Tasks.Commands.CreateTaskCommand
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(3)
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(3000)
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.DueDate.HasValue)
                .WithMessage("DueDate must be in the future");

            RuleFor(x => x.Priority)
                .IsInEnum()
                .WithMessage("Invalid priority value");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
