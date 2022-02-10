using Auth.Application;
using Auth.Infrastructure;
using Auth.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Auth.API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var identityOptions = configuration.GetSection("IdentityServer").Get<IdentityOptions>();
            
            serviceCollection
                .AddInfrastructure(identityOptions, connectionString)
                .AddApplication();

            return serviceCollection;
        }
    }
}