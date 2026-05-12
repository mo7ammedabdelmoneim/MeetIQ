using FluentValidation;
using MediatR;
using MeetIQ.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MeetIQ.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register MediatR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Register TransactionBehavior
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            return services;
        }
    }
}
