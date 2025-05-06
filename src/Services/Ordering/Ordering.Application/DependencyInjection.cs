using BuildingBlocks.Behaviours;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System.Reflection;

namespace Ordering.Application
{
    public static class DependencyInjection
    {
        // Register services related to application layer such as "MidiatR","Fluent validator", domain handler etc
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // register MidiatR, validator etc services

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            });

            services.AddFeatureManagement();
            // Registring MassTransit RabbitMQ packages
            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
