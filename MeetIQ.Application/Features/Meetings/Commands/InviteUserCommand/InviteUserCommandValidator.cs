using FluentValidation;

namespace MeetIQ.Application.Features.Meetings.Commands.InviteUserCommand
{
    public class InviteUserCommandValidator : AbstractValidator<InviteUserCommand>
    {
        public InviteUserCommandValidator()
        {
            RuleFor(x => x.MeetingId)
                .NotEmpty().WithMessage("MeetingId is required");

            RuleFor(x => x.InvitedUserId)
                .NotEmpty().WithMessage("InvitedUserId is required");

            RuleFor(x => x.InvitedByUserId)
                .NotEmpty().WithMessage("InvitedByUserId is required");

            RuleFor(x => x)
                .Must(x => x.InvitedUserId != x.InvitedByUserId)
                .WithMessage("You cannot invite yourself");
        }
    }
}