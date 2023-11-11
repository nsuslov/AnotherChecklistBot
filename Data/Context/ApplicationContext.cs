using Microsoft.EntityFrameworkCore;

namespace AnotherChecklistBot.Data.Context;

public class ApplicationContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=store.db");
    }

    public void Migrate()
    {
        var isPendingMigration = Database.GetPendingMigrations().Any();
        if (isPendingMigration)
        {
            Database.Migrate();
        }
    }
}