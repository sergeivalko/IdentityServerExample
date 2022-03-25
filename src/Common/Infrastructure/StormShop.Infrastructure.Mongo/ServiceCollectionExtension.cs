using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StormShop.Infrastructure.Mongo
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOptions>(configuration.GetSection("Mongo"));
            services.AddScoped<IMongoContext, MongoContext>();
            return services;
        } 
    }
}