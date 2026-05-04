using FluentValidation;

namespace MeetIQ.Application.Features.Feedback.Commands.CreateFeedbackCommand
{
    public class CreateFeedbackCommandValidator : AbstractValidator<CreateFeedbackCommand>
    {
        public CreateFeedbackCommandValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required")
                .MinimumLength(10).WithMessage("Message must be at least 10 characters")
                .MaximumLength(2000).WithMessage("Message cannot exceed 2000 characters");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid feedback type");

            RuleFor(x => x.ReporterId)
                .NotEmpty().WithMessage("ReporterId is required");
        }
    }
}