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

            // ------------------------------
            // Verbindung zur PostgreSQL-Datenbank:
            // Zuerst prüft Environment Variable, dann appsettings.json
            // ------------------------------
            var connectionString = Environment.GetEnvironmentVariable("SPORTMAFIASPIELDB") 
                                   ?? builder.Configuration.GetConnectionString("SportMafiaSpielDB");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection-String für PostgreSQL nicht gefunden! Setze ihn in appsettings.json oder als Environment Variable 'SPORTMAFIASPIELDB'.");
            }

            builder.Services.AddDbContext<SportMafiaSpielContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddControllers();

            var app = builder.Build();

            app.MapGet("/", () => "SportMafiaSpiel Backend läuft!");

            app.MapGet("/test-db", async (SportMafiaSpielContext db) =>
            {
                var userCount = await db.Users.CountAsync();
                return $"Benutzer in der DB: {userCount}";
            });

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SportMafiaSpielContext>();
                db.Database.Migrate();
            }

            app.Run();
        }
    }
}
