using Microsoft.EntityFrameworkCore;
using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Team> Teams { get; set; }
    public DbSet<SportType> SportTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Team and SportType relationship
        modelBuilder.Entity<Team>()
            .HasOne(t => t.SportType)      // Team has one SportType
            .WithMany()                     // SportType has many Teams
            .HasForeignKey(t => t.SportTypeId); // foreign key

        modelBuilder.Entity<SportType>()
            .ToTable("sport_types", tb => tb.ExcludeFromMigrations())
            .HasKey(s => s.Id);

        modelBuilder.Entity<SportType>()
            .Property(s => s.IsActive)
            .HasColumnName("is_active");
    }
}