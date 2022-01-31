using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Auth.Domain;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Context
{
    public class AuthContext : IdentityDbContext<User, Role, Guid>
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
            
        }
    }
}