using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tap.Domain.Features.Users;

namespace Tap.Persistence.Configurations;

internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
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

        builder
            .Property(x => x.Password)
            .HasConversion(x => x.Value, x => Password.Create(x).Value)
            .HasMaxLength(Password.MaxLength)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.Property(x => x.UpdatedAtUtc);
    }
}
