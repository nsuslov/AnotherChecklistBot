using AnotherChecklistBot.Models;
using Microsoft.EntityFrameworkCore;

namespace AnotherChecklistBot.Data.Context;

public class ApplicationContext : DbContext
{
    public DbSet<Checklist> Checklists => Set<Checklist>();
    public DbSet<ListItem> ListItems => Set<ListItem>();
    public DbSet<ChecklistMessage> ChecklistMessages => Set<ChecklistMessage>();

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