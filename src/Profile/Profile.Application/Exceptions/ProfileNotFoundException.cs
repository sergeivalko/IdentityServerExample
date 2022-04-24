using System;
using Profile.Domain.Exceptions;

namespace Profile.Application.Exceptions
{
    public class ProfileNotFoundException : BusinessException
    {
        public ProfileNotFoundException() : base(Constants.ErrorMessages.ProfileNotFound)
        {
        }

        public ProfileNotFoundException(string message) : base(message)
        {
        }

        public ProfileNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
