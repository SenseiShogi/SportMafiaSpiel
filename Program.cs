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
            // DbContext hinzufügen (Datenbankverbindung)
            // ------------------------------
            builder.Services.AddDbContext<SportMafiaSpielContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("SportMafiaSpielDB")));

            // ------------------------------
            // Controller hinzufügen
            // ------------------------------
            builder.Services.AddControllers();

            // ------------------------------
            // Swagger für API-Dokumentation hinzufügen
            // ------------------------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // ------------------------------
            // Swagger UI nur im Entwicklungsmodus aktivieren
            // ------------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
            // Test-Endpunkt, um DB-Verbindung zu prüfen
            // ------------------------------
            app.MapGet("/test-db", async (SportMafiaSpielContext db) =>
            {
                var userCount = await db.Users.CountAsync();
                return $"Benutzer in der DB: {userCount}";
            });

            // ------------------------------
            // Anwendung starten
            // ------------------------------
            app.Run();
        }
    }
}
