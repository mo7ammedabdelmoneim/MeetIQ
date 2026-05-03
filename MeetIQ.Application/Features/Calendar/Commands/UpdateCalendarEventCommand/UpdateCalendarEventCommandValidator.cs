using FluentValidation;

namespace MeetIQ.Application.Features.Calendar.Commands.UpdateCalendarEventCommand
{
    public class UpdateCalendarEventCommandValidator
        : AbstractValidator<UpdateCalendarEventCommand>
    {
        public UpdateCalendarEventCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(2).MaximumLength(200);

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Start time must be in the future.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time.")
                .Must((cmd, endTime) => (endTime - cmd.StartTime).TotalMinutes >= 15)
                .WithMessage("Event duration must be at least 15 minutes.");

            RuleFor(x => x.Color)
                .Matches(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$")
                .WithMessage("Color must be a valid hex color.");

            RuleFor(x => x.OwnerId).NotEmpty();
        }
    }
}