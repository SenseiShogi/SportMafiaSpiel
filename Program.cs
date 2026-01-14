using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportMafiaSpiel;
using SportMafiaSpiel.Models;

namespace SportMafiaSpiel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ------------------------------
            // DbContext hinzufügen (PostgreSQL-Verbindung)
            // ------------------------------
            builder.Services.AddDbContext<SportMafiaSpielContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("SportMafiaSpielDB")));

            // ------------------------------
            // Controller hinzufügen
            // ------------------------------
            builder.Services.AddControllers();

            var app = builder.Build();

            // ------------------------------
            // Health check-Endpunkt für Render
            // ------------------------------
            app.MapGet("/", () => "SportMafiaSpiel Backend is running!");

            // ------------------------------
            // Optional: Test-Endpunkt, um DB-Verbindung zu prüfen
            // ------------------------------
            app.MapGet("/test-db", async (SportMafiaSpielContext db) =>
            {
                var userCount = await db.Users.CountAsync();
                return $"Benutzer in der DB: {userCount}";
            });

            // ------------------------------
            // HTTPS-Weiterleitung aktivieren
            // ------------------------------
            app.UseHttpsRedirection();

            // ------------------------------
            // Authorization Middleware (Platzhalter)
            // ------------------------------
            app.UseAuthorization();

            // ------------------------------
            // Controller-Routen aktivieren
            // ------------------------------
            app.MapControllers();

            // ------------------------------
            // Automatische Anwendung der Migrations auf PostgreSQL
            // ------------------------------
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SportMafiaSpielContext>();
                db.Database.Migrate();
            }

            // ------------------------------
            // Anwendung starten
            // ------------------------------
            app.Run();
        }
    }
}
