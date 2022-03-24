using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Interfaces;

namespace Profile.Application.Features.UpdateProfile
{
    [UsedImplicitly]
    public class UpdateProfileHandler : INotificationHandler<UpdateProfileCommand>
    {
        private readonly IProfileUnitOfWork _profileUnitOfWork;
        private readonly IFileService _fileService;

        public UpdateProfileHandler(IProfileUnitOfWork profileUnitOfWork, IFileService fileService)
        {
            _profileUnitOfWork = profileUnitOfWork;
            _fileService = fileService;
        }

        public async Task Handle(UpdateProfileCommand notification, CancellationToken cancellationToken)
        {
            var profile = await _profileUnitOfWork.ProfileRepository.FindByIdAsync(notification.ProfileId);

            if (profile == null)
            {
                throw new ProfileNotFoundException();
            }

            if (!string.IsNullOrWhiteSpace(notification.FirstName))
            {
                profile.FirstName = notification.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(notification.LastName))
            {
                profile.LastName = notification.LastName;
            }

            if (notification.FileData != null)
            {
                var filePath = await _fileService.SaveAsync(notification.FileData);

                if (!string.IsNullOrWhiteSpace(profile.Photo))
                {
                    _fileService.Delete(profile.Photo);
                }
                
                profile.Photo = filePath;
            }

            _profileUnitOfWork.ProfileRepository.Update(profile);
            await _profileUnitOfWork.Commit();
        }
    }
}