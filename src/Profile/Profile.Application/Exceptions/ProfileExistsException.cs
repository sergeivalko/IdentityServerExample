using System;
using Profile.Domain.Exceptions;

namespace Profile.Application.Exceptions
{
    public class ProfileExistsException : BusinessException
    {
        public ProfileExistsException() : base(Constants.ErrorMessages.ProfileExists)
        {
        }

        public ProfileExistsException(string message) : base(message)
        {
        }

        public ProfileExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
