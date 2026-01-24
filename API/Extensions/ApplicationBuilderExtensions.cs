using API.Middleware;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationBuilderExtensions
    {

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();
        }

        public static IApplicationBuilder UserRequestContextLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestContextLoggingMiddleware>();

            return app;
        }
    }
}
