using System;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain
{
    public sealed class Role : IdentityRole<Guid>
    {
        public Role(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}