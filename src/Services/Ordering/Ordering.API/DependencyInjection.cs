using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        //register API specific services like carter, health check etc
        //Before building the application we use AddCarter into DI
        public static IServiceCollection AddApiServices(this IServiceCollection services,IConfiguration configuration)
        {
            //register API specific releated services like - carter
            services.AddCarter();
            ///<summary>
            ///With this line of code we will perform the adding or custom
            ///exception handling into DI container and making it available for use throughout the application.
            ///</Summary>
            services.AddExceptionHandler<CustomExceptionHandler>();
            
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database")!);

            return services;
        }

        //After building the application we will configure Http lifecycle for using carter with map carter
        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();
            ///<summary>
            ///Wrtiting below code, we set up the middleware for handling exceptions
            /// It coonfigures the application to use the custom exception handling during the request pipeline.
            ///</Summary>
            app.UseExceptionHandler(opt => { });

            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            return app;
        }
    }
}
