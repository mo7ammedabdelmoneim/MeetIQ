using MediatR;
using MeetIQ.Application.Common.Exceptions;
using MeetIQ.Application.Interfaces.Repositories;
using MeetIQ.Application.Interfaces.Services;

namespace MeetIQ.Application.Features.Profile.Commands.UpdateProfileCommand
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IFileStorageService fileStorageService;

        public UpdateProfileCommandHandler(
            IUserRepository userRepository,
            IFileStorageService fileStorageService)
        {
            this.userRepository = userRepository;
            this.fileStorageService = fileStorageService;
        }

        public async Task<bool> Handle(
            UpdateProfileCommand request,
            CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(x => x.Id == request.UserId);

            if (user == null)
                throw new NotFoundException("User not found");

            user.FullName = request.FullName;

            // Handle avatar
            if (request.RemoveAvatar && !string.IsNullOrEmpty(user.AvatarUrl))
            {
                await fileStorageService.DeleteAsync(user.AvatarUrl);
                user.AvatarUrl = null;
            }
            else if (request.AvatarFile != null)
            {
                // Delete old avatar first
                if (!string.IsNullOrEmpty(user.AvatarUrl))
                    await fileStorageService.DeleteAsync(user.AvatarUrl);

                user.AvatarUrl = await fileStorageService.UploadAsync(
                    request.AvatarFile,
                    folder: "avatars");
            }

            userRepository.Update(user);
            await userRepository.SaveChangesAsync();

            return true;
        }
    }
}