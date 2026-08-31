using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.Data;

public class SportTypeConfiguration : IEntityTypeConfiguration<SportType>
{
    public void Configure(EntityTypeBuilder<SportType> builder)
    {
        builder.ToTable("sport_types", tb => tb.ExcludeFromMigrations());

        builder.HasKey(s => s.Id);

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active");
    }
}