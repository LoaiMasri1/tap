using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tap.Domain.Features.Cities;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Users;

namespace Tap.Persistence.Configurations;

internal class HotelEntityConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable(nameof(Hotel));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();

        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();

        builder.OwnsOne(
            hotel => hotel.Location,
            locationBuilder =>
            {
                locationBuilder.WithOwner();

                locationBuilder.Property(x => x.Latitude).HasColumnName("Latitude").IsRequired();
                locationBuilder.Property(x => x.Longitude).HasColumnName("Longitude").IsRequired();
            }
        );

        builder.Property(x => x.Rating).IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.Property(x => x.UpdatedAtUtc);

        builder
            .HasOne<City>()
            .WithMany(x => x.Hotels)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<User>()
            .WithMany(x => x.Hotels)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Rooms)
            .WithOne(x => x.Hotel)
            .HasForeignKey(x => x.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
