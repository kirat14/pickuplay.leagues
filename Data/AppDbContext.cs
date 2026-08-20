using Microsoft.EntityFrameworkCore;
using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<League> Leagues { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<SportType> SportTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<League>()
        .HasOne(l => l.SportType)
        .WithMany()
        .HasForeignKey(l => l.SportTypeId);

        modelBuilder.Entity<Team>()
            .HasOne(t => t.League)
            .WithMany(l => l.Teams)
            .HasForeignKey(t => t.LeagueId);


        modelBuilder.Entity<SportType>()
            .ToTable("sport_types", tb => tb.ExcludeFromMigrations())
            .HasKey(s => s.Id);

        modelBuilder.Entity<SportType>()
            .Property(s => s.IsActive)
            .HasColumnName("is_active");
    }
}