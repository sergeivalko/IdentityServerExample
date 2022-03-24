using Auth.Application.Behaviors;
using Auth.Application.Features.Commands.CreateAccount;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(DependencyInjection));
            serviceCollection.AddValidatorsFromAssemblyContaining<CreateAccountRequestValidator>();
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return serviceCollection;
        }
    }
}