using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tap.Domain.Features.Amenities;

namespace Tap.Persistence.Configurations;

public class AmenityEntityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.ToTable(nameof(Amenity));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();

        builder
            .Property(x => x.Type)
            .HasConversion(v => v.ToString(), v => (AmenityType)Enum.Parse(typeof(AmenityType), v));

        builder.Property(x => x.TypeId).IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.Property(x => x.UpdatedAtUtc);
    }
}
