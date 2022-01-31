using System;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain
{
    public sealed class User : IdentityUser<Guid>
    {
        public DateTimeOffset Created { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.UtcNow;
        }
    }
}