using FluentValidation;

namespace MeetIQ.Application.Features.Notes.Commands.CreateNoteCommand
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(2)
                .MaximumLength(300);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required");

            RuleFor(x => x.AuthorId)
                .NotEmpty().WithMessage("AuthorId is required");

            RuleForEach(x => x.Tags)
                .MaximumLength(50)
                .When(x => x.Tags.Any());
        }
    }
}
