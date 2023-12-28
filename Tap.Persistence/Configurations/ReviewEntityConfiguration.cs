using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tap.Domain.Features.Hotels;
using Tap.Domain.Features.Reviews;
using Tap.Domain.Features.Users;

namespace Tap.Persistence.Configurations;

public class ReviewEntityConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(nameof(Review));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Content).HasMaxLength(500).IsRequired();

        builder.Property(x => x.Rating).IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.Property(x => x.UpdatedAtUtc);

        builder
            .HasOne<Hotel>()
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
