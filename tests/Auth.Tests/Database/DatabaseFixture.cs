using System;
using Auth.Domain;
using Auth.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StormShop.Common;

#pragma warning disable

namespace Auth.Tests.Database
{
    public class DatabaseFixture
    {
        public AuthContext CreateStore()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AuthContext>();
            dbContextOptions.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));
            var context = new AuthContext(dbContextOptions.Options);

            context.Add(new Role(AuthRoles.Admin)
            {
                NormalizedName = AuthRoles.Admin.ToUpperInvariant()
            });
            context.Add(new Role(AuthRoles.Customer)
            {
                NormalizedName = AuthRoles.Customer.ToUpperInvariant()
            });

            context.SaveChanges();
            
            return context;
        }
    }
}
