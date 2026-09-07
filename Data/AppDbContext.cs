using Microsoft.EntityFrameworkCore;

using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Competition> Competitions { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<SportType> SportTypes { get; set; }
    public DbSet<CompetitionTeamEntry> CompetitionTeamEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}