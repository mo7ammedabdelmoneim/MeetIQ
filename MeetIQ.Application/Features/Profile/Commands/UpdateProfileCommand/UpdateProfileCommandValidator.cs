using FluentValidation;

namespace MeetIQ.Application.Features.Profile.Commands.UpdateProfileCommand
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
        private const long MaxFileSize = 2 * 1024 * 1024; // 2 MB

        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MinimumLength(2).WithMessage("Full name must be at least 2 characters")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            When(x => x.AvatarFile != null, () =>
            {
                RuleFor(x => x.AvatarFile!.Length)
                    .LessThanOrEqualTo(MaxFileSize)
                    .WithMessage("Avatar image must be 2 MB or less");

                RuleFor(x => Path.GetExtension(x.AvatarFile!.FileName).ToLower())
                    .Must(ext => AllowedExtensions.Contains(ext))
                    .WithMessage("Avatar must be JPG, PNG, or WebP");
            });
        }
    }
}