using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pickuplay.Teams.Data;

public class CompetitionTeamEntryConfiguration : IEntityTypeConfiguration<CompetitionTeamEntry>
{
    public void Configure(EntityTypeBuilder<CompetitionTeamEntry> builder)
    {
        builder.ToTable("competition_team_entries");

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

        builder.HasOne(e => e.Competition)
            .WithMany(l => l.Entries)
            .HasForeignKey(e => e.CompetitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Player)
        .WithMany()
        .HasForeignKey(e => e.PlayerId);

        builder.HasIndex(e => new { e.PlayerId, e.CompetitionId })
        .IsUnique();
    }
}