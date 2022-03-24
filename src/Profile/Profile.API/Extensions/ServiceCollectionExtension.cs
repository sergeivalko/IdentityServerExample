using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Application;
using Profile.Infrastructure;

namespace Profile.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {

            services
                .AddApplication()
                .AddInfrastructure(configuration);
            
            return services;
        }
    }
}