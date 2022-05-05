using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Profile.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddMediatR(typeof(ServiceCollectionExtension))
                .AddAutoMapper(typeof(ServiceCollectionExtension));
            
            return services;
        }
    }
}
