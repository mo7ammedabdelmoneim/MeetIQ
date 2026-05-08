using FluentValidation;
using MeetIQ.Domain.Enums;

namespace MeetIQ.Application.Features.Meetings.Commands.RespondToInvitationCommand
{
    public class RespondToInvitationCommandValidator : AbstractValidator<RespondToInvitationCommand>
    {
        public RespondToInvitationCommandValidator()
        {
            RuleFor(x => x.InvitationId)
                .NotEmpty().WithMessage("InvitationId is required");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.Response)
                .Must(r => r == InvitationStatus.Accepted || r == InvitationStatus.Declined)
                .WithMessage("Response must be Accepted or Declined");
        }
    }
}