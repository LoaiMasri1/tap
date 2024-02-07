using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Primitives;

namespace Tap.Domain.Features.Discounts;

public class Discount : Entity, IAuditableEntity
{
    private Discount() { }

    private Discount(string name, string description, int discountPercentage)
    {
        Name = name;
        Description = description;
        DiscountPercentage = discountPercentage;
    }

    public static Discount Create(string name, string description, int discountPercentage)
    {
        return new Discount(name, description, discountPercentage);
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public int DiscountPercentage { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public Money Apply(Money price)
    {
        var discountedPrice = price.Amount - (price.Amount * DiscountPercentage / 100);

        return Money.Create(discountedPrice, price.Currency);
    }
}
