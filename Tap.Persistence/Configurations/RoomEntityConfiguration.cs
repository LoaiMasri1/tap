using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tap.Domain.Features.Rooms;

namespace Tap.Persistence.Configurations;

public class RoomEntityConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable(nameof(Room));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number).IsRequired();

        builder.OwnsOne(
            room => room.Price,
            priceBuilder =>
            {
                priceBuilder.WithOwner();

                priceBuilder
                    .Property(x => x.Amount)
                    .HasColumnName("Price")
                    .IsRequired()
                    .HasPrecision(18, 2)
                    .HasColumnType("decimal(18,2)");
                priceBuilder
                    .Property(x => x.Currency)
                    .HasColumnName("Currency")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnType("char(3)");
            }
        );

        builder.Property(x => x.DiscountedPrice).IsRequired();

        builder
            .Property(x => x.Type)
            .HasConversion(v => v.ToString(), v => (RoomType)Enum.Parse(typeof(RoomType), v));

        builder.Property(x => x.IsAvailable).IsRequired();

        builder.Property(x => x.CapacityOfAdults).IsRequired();

        builder.Property(x => x.CapacityOfChildren).IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.Property(x => x.UpdatedAtUtc);

        builder.HasMany(x => x.Discounts).WithOne().OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Hotel).WithMany(x => x.Rooms).HasForeignKey(x => x.HotelId);
    }
}
