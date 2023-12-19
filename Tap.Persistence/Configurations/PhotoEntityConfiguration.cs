using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tap.Domain.Common.Enumerations;
using Tap.Domain.Features.Photos;

namespace Tap.Persistence.Configurations;

public class PhotoEntityConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable(nameof(Photo));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Url).HasMaxLength(500).IsRequired();

        builder
            .Property(x => x.Type)
            .HasConversion(x => x.ToString(), x => (ItemType)Enum.Parse(typeof(ItemType), x));

        builder.Property(x => x.ItemId).IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.Property(x => x.UpdatedAtUtc);
    }
}
