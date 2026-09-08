using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pickuplay.Teams.Models;

namespace Pickuplay.Teams.Data;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("users", tb => tb.ExcludeFromMigrations());

        builder.HasKey(p => p.Id);


        builder.Property(p => p.FirstName)
            .HasColumnName("first_name");

        builder.Property(p => p.LastName)
            .HasColumnName("last_name");

        builder.Property(p => p.SkillLevel)
            .HasColumnName("skill_level");
    }
}