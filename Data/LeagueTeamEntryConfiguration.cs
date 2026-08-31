using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pickuplay.Teams.Data;

public class LeagueTeamEntryConfiguration : IEntityTypeConfiguration<LeagueTeamEntry>
{
    public void Configure(EntityTypeBuilder<LeagueTeamEntry> builder)
    {
        builder.ToTable("league_team_entries");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.IsTeam)
            .HasDefaultValue(false);

        builder.Property(e => e.GuestCount)
            .HasDefaultValue(0);

        builder.Property(e => e.Status)
            .HasConversion<string>()   // stores enum as string in MySQL
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.JoinedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        builder.HasOne(e => e.Team)
            .WithMany(t => t.Entries)
            .HasForeignKey(e => e.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.League)
            .WithMany(l => l.Entries)
            .HasForeignKey(e => e.LeagueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.PlayerId, e.LeagueId })
        .IsUnique();
    }
}