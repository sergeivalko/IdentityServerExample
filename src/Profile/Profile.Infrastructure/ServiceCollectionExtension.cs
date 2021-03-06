using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Application.Interfaces;
using Profile.Infrastructure.Database;
using StormShop.Infrastructure.Mongo;

namespace Profile.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            MongoDbConfiguration.Configure();

            services.Configure<KafkaOptions>(configuration.GetSection("Kafka"));

            services
                .AddMongo(configuration)
                .AddScoped<IProfileUnitOfWork, ProfileUnitOfWork>();

            services.AddMediatR(typeof(ServiceCollectionExtension));
            services.AddSingleton<IDateProvider, DateTimeUtcProvider>();

            services.AddHostedService<AccountCreatedConsumer>();
            return services;
        }
    }
}
