using FluentValidation;

namespace MeetIQ.Application.Features.Notes.Commands.UpdateNoteCommand
{
    public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(300);
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required");
            RuleFor(x => x.RequesterId)
                .NotEmpty();
        }
    }
}
