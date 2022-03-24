using Profile.Application.Features.GetProfile;

namespace Profile.Application.Mappers
{
    public class ProfileMapper: AutoMapper.Profile
    {
        public ProfileMapper()
        {
            CreateMap<Domain.Profile, GetProfileResult>();
        }
    }
}