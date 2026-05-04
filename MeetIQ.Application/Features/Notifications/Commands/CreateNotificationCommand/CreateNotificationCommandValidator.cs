using FluentValidation;

namespace MeetIQ.Application.Features.Notifications.Commands.CreateNotificationCommand
{
    public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
    {
        public CreateNotificationCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200);

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required")
                .MaximumLength(1000);

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid notification type");
        }
    }
}