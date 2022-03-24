using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Interfaces;

namespace Profile.Application.Features.GetProfile
{
    public class GetProfileHandler: IRequestHandler<GetProfileQuery, GetProfileResult>
    {
        private readonly IProfileUnitOfWork _profileUnitOfWork;
        private readonly IMapper _mapper;

        public GetProfileHandler(IProfileUnitOfWork profileUnitOfWork, IMapper mapper)
        {
            _profileUnitOfWork = profileUnitOfWork;
            _mapper = mapper;
        }

        public async Task<GetProfileResult> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await _profileUnitOfWork.ProfileRepository.FindByIdAsync(request.ProfileId);

            if (profile == null)
            {
                throw new ProfileNotFoundException();
            }

            var profileResult = _mapper.Map<GetProfileResult>(profile);
            return profileResult;
        }
    }
}