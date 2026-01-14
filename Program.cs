using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportMafiaSpiel;

namespace SportMafiaSpiel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DbContext (PostgreSQL)
            builder.Services.AddDbContext<SportMafiaSpielContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("SportMafiaSpielDB"),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.EnableRetryOnFailure();
                    }
                )
            );

            builder.Services.AddControllers();

            var app = builder.Build();

            app.MapGet("/", () => "SportMafiaSpiel Backend is running!");

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Apply migrations
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SportMafiaSpielContext>();
                db.Database.Migrate();
            }

            app.MapGet("/test-db", async (SportMafiaSpielContext db) =>
            {
                var userCount = await db.Users.CountAsync();
                return $"Users in DB: {userCount}";
            });

            app.Run();
        }
    }
}
