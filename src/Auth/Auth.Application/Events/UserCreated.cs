using System;

namespace Auth.Application.Events
{
    public class UserCreated
    {
        public Guid AccountId { get; set; }
    }
}