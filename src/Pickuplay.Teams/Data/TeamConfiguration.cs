using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.Data;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("teams");

        builder.HasKey(t => t.Id);

        builder.HasOne(t => t.Competition)
        .WithMany(l => l.Teams)
        .HasForeignKey(t => t.CompetitionId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.Name)
        .IsRequired();

        builder.HasIndex(t => new { t.CompetitionId, t.Name })
        .IsUnique();

    }
}