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
            // Zuerst prüft Environment Variable, sonst wird Render DB direkt genutzt
            // ------------------------------
            string? connectionString = Environment.GetEnvironmentVariable("SPORTMAFIASPIELDB")
                                     ?? "Host=dpg-d5jkmsali9vc73bh94vg-a;Database=sportmafiaspiel_db;Username=sportmafiaspiel_db_user;Password=hfNx3dA3mMDtJz57sjm4AO4RYhonlR6S;Port=5432;SSL Mode=Require;Trust Server Certificate=true;Pooling=true;";

            // ------------------------------
            // Dummy-Modus für lokale Tests
            // ------------------------------
            bool useDummyDatabase = false;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                useDummyDatabase = true;
                Console.WriteLine("WARNUNG: Keine ENV Variable 'SPORTMAFIASPIELDB' gefunden. Dummy-Datenbank wird verwendet.");
                connectionString = "Host=localhost;Database=dummy;Username=dummy;Password=dummy;";
            }

            // ------------------------------
            // DbContext konfigurieren
            // ------------------------------
            builder.Services.AddDbContext<SportMafiaSpielContext>(options =>
            {
                options.UseNpgsql(connectionString);

                if (useDummyDatabase)
                {
                    // Dummy-Modus: keine Migrationen, Logging aktiviert
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
            // Test-Endpunkt für DB-Verbindung
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
            // Automatische Migrationen nur für echte DB
            // ------------------------------
            if (!useDummyDatabase)
            {
                try
                {
                    using (var scope = app.Services.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<SportMafiaSpielContext>();
                        db.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("FEHLER: Migrationen konnten nicht angewendet werden: " + ex.Message);
                    throw;
                }
            }

            // ------------------------------
            // Anwendung starten
            // ------------------------------
            app.Run();
        }
    }
}
