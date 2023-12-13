using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tap.Application.Core.Abstractions.Common;
using Tap.Domain.Features.Users;

namespace Tap.Persistence.Configurations;

internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    private IDateTime _dateTime;

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();

        builder
            .Property(x => x.Email)
            .HasConversion(x => x.Value, x => Email.Create(x).Value)
            .HasMaxLength(Email.MaxLength)
            .IsRequired();

        builder.OwnsOne(
            user => user.ActivationToken,
            tokenBuilder =>
            {
                tokenBuilder.WithOwner();

                tokenBuilder.Property(x => x.Value).HasColumnName("TokenValue");
                tokenBuilder.Property(x => x.ExpiredAt).HasColumnName("TokenExpireAt");
            }
        );

        builder
            .Property<string>("_hashedPassword")
            .HasField("_hashedPassword")
            .HasColumnName("Password")
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.Property(x => x.UpdatedAtUtc);
    }
}
