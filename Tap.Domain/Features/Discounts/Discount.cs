using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Primitives;

namespace Tap.Domain.Features.Discounts;

public class Discount : Entity, IAuditableEntity
{
    private Discount() { }

    private Discount(
        string name,
        string description,
        int discountPercentage,
        DateTime startDate,
        DateTime endDate
    )
    {
        Name = name;
        Description = description;
        DiscountPercentage = discountPercentage;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Discount Create(
        string name,
        string description,
        int discountPercentage,
        DateTime startDate,
        DateTime endDate
    )
    {
        return new Discount(name, description, discountPercentage, startDate, endDate);
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public int DiscountPercentage { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public bool IsApplicable(DateTime now) => StartDate <= now && EndDate >= now;

    public Money Apply(Money price)
    {
        var discountedPrice = price.Amount - (price.Amount * DiscountPercentage / 100);

        return Money.Create(discountedPrice, price.Currency);
    }
}
