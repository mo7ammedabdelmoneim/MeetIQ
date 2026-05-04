using FluentValidation;

namespace MeetIQ.Application.Features.Feedback.Commands.UpdateFeedbackStatusCommand
{
    public class UpdateFeedbackStatusCommandValidator : AbstractValidator<UpdateFeedbackStatusCommand>
    {
        public UpdateFeedbackStatusCommandValidator()
        {
            RuleFor(x => x.FeedbackId)
                .NotEmpty().WithMessage("FeedbackId is required");

            RuleFor(x => x.NewStatus)
                .IsInEnum().WithMessage("Invalid feedback status");
        }
    }
}