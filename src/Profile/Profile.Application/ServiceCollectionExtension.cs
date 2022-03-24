using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Profile.Application.Features.UpdateProfile;

namespace Profile.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddMediatR(typeof(ServiceCollectionExtension))
                .AddValidators()
                .AddAutoMapper(typeof(ServiceCollectionExtension));
            
            return services;
        }

        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<UpdateProfileRequestDtoValidator>();
            return services;
        }
    }
}