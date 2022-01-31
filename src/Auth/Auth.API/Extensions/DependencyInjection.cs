using Auth.Application;
using Auth.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Auth.API.Extensions
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencies(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            serviceCollection
                .AddInfrastructure(connectionString)
                .AddApplication();
        }
    }
}