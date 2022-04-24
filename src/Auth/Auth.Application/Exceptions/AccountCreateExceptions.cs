using System;
using Auth.Domain.Exceptions;

namespace Auth.Application.Exceptions
{
    public class AccountCreateExceptions : BusinessException
    {
        public AccountCreateExceptions() : base()
        {
        }

        public AccountCreateExceptions(string message) : base(message)
        {
        }

        public AccountCreateExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
