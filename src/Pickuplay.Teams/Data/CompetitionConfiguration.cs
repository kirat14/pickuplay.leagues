using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.Data;

public class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
{
    public void Configure(EntityTypeBuilder<Competition> builder)
    {
        builder.ToTable("competitions");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(l => l.City)
            .HasMaxLength(255);

        builder.Property(l => l.Address)
            .HasMaxLength(255);

        builder.Property(l => l.Description)
            .HasMaxLength(2000);

        builder.Property(l => l.StartDate)
            .IsRequired();

        builder.Property(l => l.StartRegistration)
            .IsRequired();

        builder.Property(l => l.EndRegistration)
            .IsRequired();

        builder.Property(l => l.NbrOfTeams)
            .IsRequired();

        builder.Property(l => l.TeamSize)
            .IsRequired();

        builder.Property(l => l.NbrOfSubs)
            .IsRequired();

        builder.Property(l => l.PricePlayer)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(l => l.Gender)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(l => l.MinimumAge)
            .IsRequired(false);

        builder.Property(l => l.Format)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(l => l.OrganizerId)
            .IsRequired();

        builder.Property(l => l.Logo)
            .HasMaxLength(128);

        builder.Property(l => l.CoverPhoto)
            .HasMaxLength(128);

        builder.Property(l => l.TeamSize)
            .IsRequired();

        builder.Property(l => l.NbrOfSubs)
            .IsRequired();

        builder.Property(l => l.Referee)
            .HasDefaultValue(false);

        builder.Property(l => l.Prize)
            .HasDefaultValue(false);

        builder.Property(l => l.Pennies)
            .HasDefaultValue(false);

        builder.HasMany(l => l.Teams)
            .WithOne(t => t.Competition)
            .HasForeignKey(t => t.CompetitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.SportType)
        .WithMany()
        .HasForeignKey(l => l.SportTypeId);

        builder.Navigation(l => l.SportType)
        .AutoInclude();
    }
}