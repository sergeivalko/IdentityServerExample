using System;
using Profile.Domain.Exceptions;

namespace Profile.Application.Exceptions
{
    public class ProfileAlreadyExistsException : BusinessException
    {
        public ProfileAlreadyExistsException() : base(Constants.ErrorMessages.ProfileExists)
        {
        }

        public ProfileAlreadyExistsException(string message) : base(message)
        {
        }

        public ProfileAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
