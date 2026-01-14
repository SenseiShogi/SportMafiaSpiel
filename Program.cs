using System;
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
            string? connectionString = Environment.GetEnvironmentVariable("SPORTMAFIASPIELDB")
                                     ?? builder.Configuration.GetConnectionString("SportMafiaSpielDB");

            // ------------------------------
            // Lokaler Testmodus: keine echte Datenbank
            // ------------------------------
            bool useDummyDatabase = false;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                useDummyDatabase = true;
                Console.WriteLine("WARNUNG: Environment Variable 'SPORTMAFIASPIELDB' nicht gesetzt. Verwende Dummy-Datenbank für lokalen Test.");
                connectionString = "Host=localhost;Database=dummy;Username=dummy;Password=dummy;";
            }

            // ------------------------------
            // DbContext hinzufügen (PostgreSQL)
            // ------------------------------
            builder.Services.AddDbContext<SportMafiaSpielContext>(options =>
            {
                options.UseNpgsql(connectionString);
                if (useDummyDatabase)
                {
                    // Verhindert Migrationsversuche auf Dummy-Datenbank
                    options.EnableSensitiveDataLogging();
                }
            });

            // ------------------------------
            // Controller hinzufügen
            // ------------------------------
            builder.Services.AddControllers();

            var app = builder.Build();

            // ------------------------------
            // Health-Check-Endpunkt
            // ------------------------------
            app.MapGet("/health", () => "SportMafiaSpiel Backend läuft!");

            // ------------------------------
            // Test-Endpunkt, um DB-Verbindung zu prüfen
            // ------------------------------
            app.MapGet("/test-db", async (SportMafiaSpielContext db) =>
            {
                try
                {
                    var userCount = await db.Users.CountAsync();
                    return $"Benutzer in der DB: {userCount}";
                }
                catch
                {
                    return "Keine Verbindung zur Datenbank verfügbar (Dummy-Modus).";
                }
            });

            // ------------------------------
            // HTTPS-Weiterleitung und Authorization
            // ------------------------------
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // ------------------------------
            // Controller-Routen aktivieren
            // ------------------------------
            app.MapControllers();

            // ------------------------------
            // Automatische Migrationen anwenden
            // ------------------------------
            if (!useDummyDatabase)
            {
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<SportMafiaSpielContext>();
                    db.Database.Migrate();
                }
            }

            // ------------------------------
            // Anwendung starten
            // ------------------------------
            app.Run();
        }
    }
}
