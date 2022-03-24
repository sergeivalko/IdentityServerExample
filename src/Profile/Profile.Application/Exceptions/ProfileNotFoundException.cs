using Profile.Domain.Exceptions;

namespace Profile.Application.Exceptions
{
    public class ProfileNotFoundException : BusinessException
    {
        public ProfileNotFoundException() : base(Constants.ErrorMessages.ProfileNotFound)
        {
        }
    }
}