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
            // Verbindung zur PostgreSQL-Datenbank aus Environment Variable oder appsettings.json
            // ------------------------------
            var connectionString = builder.Configuration.GetConnectionString("SportMafiaSpielDB") 
                                   ?? Environment.GetEnvironmentVariable("SPORTMAFIASPIELDB");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception(
                    "Connection-String für PostgreSQL nicht gefunden! " +
                    "Setze ihn in appsettings.json oder als Environment Variable 'SPORTMAFIASPIELDB'."
                );
            }

            // ------------------------------
            // DbContext hinzufügen (PostgreSQL-Verbindung)
            // ------------------------------
            builder.Services.AddDbContext<SportMafiaSpielContext>(options =>
                options.UseNpgsql(connectionString));

            // ------------------------------
            // Controller hinzufügen
            // ------------------------------
            builder.Services.AddControllers();

            var app = builder.Build();

            // ------------------------------
            // Health-Check-Endpunkt für Render
            // ------------------------------
            app.MapGet("/", () => "SportMafiaSpiel Backend läuft!");

            // ------------------------------
            // Test-Endpunkt, um DB-Verbindung zu prüfen
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
            // Automatische Anwendung der Migrationen auf PostgreSQL
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
