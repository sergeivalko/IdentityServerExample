using Auth.Domain.Exceptions;

namespace Auth.Application.Exceptions
{
    public class AccountCreateExceptions : BusinessException
    {
        public AccountCreateExceptions(string message) : base(message)
        {
        }
    }
}