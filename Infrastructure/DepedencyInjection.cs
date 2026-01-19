using Application.Abstraction.Clock;
using Application.Abstraction.Data;
using Application.Abstraction.Email;
using Domain.Abstraction;
using Domain.Apartments;
using Domain.Bookings;
using Domain.User;
using Infrastructure.Clock;
using Infrastructure.Data;
using Infrastructure.Email;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            services.AddTransient<IEmailService, EmailService>();

            var connectionstring =
                configuration.GetConnectionString("DefaultConnection") ??
                throw new ArgumentException(nameof(configuration));

            services.AddDbContext<ApplicationDbContext>(
            options => options.UseNpgsql(connectionstring))
               ;

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());


            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionstring));

            return services;
        }
    }
}
