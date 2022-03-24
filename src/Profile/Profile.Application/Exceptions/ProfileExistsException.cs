using Profile.Domain.Exceptions;

namespace Profile.Application.Exceptions
{
    public class ProfileExistsException : BusinessException
    {
        public ProfileExistsException() : base(Constants.ErrorMessages.ProfileExists)
        {
        }
    }
}