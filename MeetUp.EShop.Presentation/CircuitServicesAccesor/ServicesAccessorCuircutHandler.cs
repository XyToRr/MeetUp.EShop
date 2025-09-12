using Microsoft.AspNetCore.Components.Server.Circuits;

namespace MeetUp.EShop.Presentation.CircuitServicesAccesor
{
    public class ServicesAccessorCuircutHandler (IServiceProvider services, 
        CircuitServicesAccesor servicesAccesor) : CircuitHandler
    {
        public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
            Func<CircuitInboundActivityContext, Task> next) =>
            async context =>
            {
                servicesAccesor.Service = services;
                await next(context);
                servicesAccesor.Service = null;
            };

    }

    public static class CircuitServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddCircuitServicesAccesor(
            this IServiceCollection services)
        {
            services.AddScoped<CircuitServicesAccesor>();
            services.AddScoped<CircuitHandler, ServicesAccessorCuircutHandler>();

            return services;
        }
    }
}
