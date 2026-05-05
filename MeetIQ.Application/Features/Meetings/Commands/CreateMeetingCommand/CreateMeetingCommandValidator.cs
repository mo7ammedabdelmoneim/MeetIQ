using FluentValidation;

namespace MeetIQ.Application.Features.Meetings.Commands.CreateMeetingCommand
{
    public class CreateMeetingCommandValidator : AbstractValidator<CreateMeetingCommand>
    {
        public CreateMeetingCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.ScheduledAt)
                .GreaterThan(DateTime.UtcNow).WithMessage("Meeting must be scheduled in the future");

            RuleFor(x => x.HostId)
                .NotEmpty().WithMessage("HostId is required");
        }
    }
}