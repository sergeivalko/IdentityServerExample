using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Interfaces;

namespace Profile.Application.Features.CreateProfile
{
    [UsedImplicitly]
    public class CreateProfileHandler : INotificationHandler<CreateProfileCommand>
    {
        private readonly IProfileUnitOfWork _profileUnitOfWork;
        private readonly IDateProvider _dateProvider;

        public CreateProfileHandler(IProfileUnitOfWork profileUnitOfWork, IDateProvider dateProvider)
        {
            _profileUnitOfWork = profileUnitOfWork;
            _dateProvider = dateProvider;
        }

        public async Task Handle(CreateProfileCommand notification, CancellationToken cancellationToken)
        {
            var existsProfile =
                await _profileUnitOfWork.ProfileRepository.FindOneAsync(x => x.AccountId == notification.AccountId);

            if (existsProfile != null)
            {
                throw new ProfileAlreadyExistsException();
            }

            var profile = new Domain.Profile(Guid.NewGuid(), _dateProvider.Now)
            {
                AccountId = notification.AccountId
            };

            _profileUnitOfWork.ProfileRepository.Add(profile);
            await _profileUnitOfWork.Commit();
        }
    }
}