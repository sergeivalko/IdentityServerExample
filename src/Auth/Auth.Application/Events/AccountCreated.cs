using System;

namespace Auth.Application.Events
{
    public class AccountCreated
    {
        public Guid AccountId { get; set; }
    }
}
