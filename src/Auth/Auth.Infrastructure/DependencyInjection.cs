using System;
using Auth.Domain;
using Auth.Infrastructure.Context;
using Auth.Infrastructure.Seed;
using Auth.Infrastructure.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityOptions = Auth.Infrastructure.Options.IdentityOptions;

namespace Auth.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
            IdentityOptions identityOptions, string connectionString)
        {
            serviceCollection.AddDbContext<AuthContext>(options =>
                {
                    options.UseSqlServer(connectionString,
                        sqlOptions => { sqlOptions.MigrationsAssembly(typeof(AuthContext).Assembly.FullName); });
                }).AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
                })
                .AddEntityFrameworkStores<AuthContext>()
                .AddDefaultTokenProviders();

            serviceCollection.AddTransient<IProfileService, ProfileService>();
            serviceCollection.AddIdentityConfiguration(identityOptions);

            return serviceCollection;
        }

        public static void CallDbMigrateExtension(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<AuthContext>();

            context?.Database?.Migrate();

            var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<AuthContext>>();
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

            new DatabaseSeed().SeedAsync(userManager, roleManager, logger, 1).Wait();
        }

        private static void AddIdentityConfiguration(this IServiceCollection serviceCollection,
            IdentityOptions identityOptions)
        {
            serviceCollection.AddIdentityServer(x => { x.Authentication.CookieLifetime = TimeSpan.FromHours(1); })
                .AddAspNetIdentity<User>()
                .AddInMemoryClients(identityOptions.Clients)
                .AddInMemoryApiResources(identityOptions.ApiResources)
                .AddInMemoryIdentityResources(identityOptions.IdentityResources)
                .AddInMemoryApiScopes(identityOptions.ApiScopes)
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<OwnerResourcePasswordValidator>();
        }
    }
}