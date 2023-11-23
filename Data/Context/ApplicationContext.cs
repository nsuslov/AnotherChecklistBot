using AnotherChecklistBot.Models;
using Microsoft.EntityFrameworkCore;

namespace AnotherChecklistBot.Data.Context;

public class ApplicationContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Checklist> Checklists => Set<Checklist>();
    public DbSet<ListItem> ListItems => Set<ListItem>();
    public DbSet<ChecklistMessage> ChecklistMessages => Set<ChecklistMessage>();

    public ApplicationContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_configuration.GetConnectionString("Default"));
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