using Microsoft.EntityFrameworkCore;

namespace SportMafiaSpiel
{
    // DbContext für SportMafiaSpiel-Datenbank
    public class SportMafiaSpielContext : DbContext
    {
        public SportMafiaSpielContext(DbContextOptions<SportMafiaSpielContext> options) 
            : base(options) { }

        // Tabellen in der Datenbank
        public DbSet<SportMafiaSpiel.Models.User> Users => Set<SportMafiaSpiel.Models.User>();
        public DbSet<SportMafiaSpiel.Models.Game> Games => Set<SportMafiaSpiel.Models.Game>();
        public DbSet<SportMafiaSpiel.Models.Session> Sessions => Set<SportMafiaSpiel.Models.Session>();
        public DbSet<SportMafiaSpiel.Models.Result> Results => Set<SportMafiaSpiel.Models.Result>();
    }
}
