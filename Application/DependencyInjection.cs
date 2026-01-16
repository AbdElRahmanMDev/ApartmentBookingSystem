using Domain.Bookings;
using Microsoft.Extensions.DependencyInjection;


namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
        });

        services.AddTransient<PricingService>();
        return services;
    }
}
