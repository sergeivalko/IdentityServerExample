using Auth.Application;
using Auth.Application.Events;
using Auth.Infrastructure;
using Auth.Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StormShop.Infrastructure.Kafka;

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
                .AddApplication()
                .AddInfrastructure(identityOptions, connectionString)
                .AddKafka(configuration);

            return serviceCollection;
        }


        private static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddProducer<UserCreated>(configuration, "NewUsers");
            return services;
        }
    }
}