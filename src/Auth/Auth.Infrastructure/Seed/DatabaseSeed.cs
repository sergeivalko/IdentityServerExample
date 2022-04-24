using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Domain;
using Auth.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using StormShop.Common;

namespace Auth.Infrastructure.Seed
{
    public class DatabaseSeed
    {
        public async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<AuthContext> logger, int retry = 0)
        {
            int retryForAvaiability = retry;

            try
            {
                await CreateRoles(roleManager);

                if (!userManager.Users.Any())
                {
                    var user = new User()
                    {
                        Id = Guid.NewGuid(),
                        Email = "johndoe@mail.ru",
                        UserName = "johndoe"
                    };

                    await userManager.CreateAsync(user, "111111");
                    await userManager.AddToRoleAsync(user, AuthRoles.Admin);
                    await userManager.AddToRoleAsync(user, AuthRoles.Customer);
                }

            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "Exception Error while migrating {DbContextName}",
                        nameof(AuthContext));

                    await SeedAsync(userManager, roleManager, logger, retryForAvaiability);
                }
            }
        }

        private static async Task CreateRoles(RoleManager<Role> roleManager)
        {
            var customerRoleExists = await roleManager.RoleExistsAsync(AuthRoles.Customer);
            if (!customerRoleExists)
            {
                var touristRole = new Role(AuthRoles.Customer);
                await roleManager.CreateAsync(touristRole);
            }

            var adminRoleExists = await roleManager.RoleExistsAsync(AuthRoles.Admin);
            if (!adminRoleExists)
            {
                var adminRole = new Role(AuthRoles.Admin);
                await roleManager.CreateAsync(adminRole);
            }
        }
    }
}
