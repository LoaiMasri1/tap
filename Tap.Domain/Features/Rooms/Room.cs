using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Features.Bookings;
using Tap.Domain.Features.Discounts;
using Tap.Domain.Features.Hotels;

namespace Tap.Domain.Features.Rooms;

public class Room : Entity, IAuditableEntity
{
    private Room() { }

    private Room(
        int number,
        Money price,
        RoomType type,
        bool isAvailable,
        int capacityOfAdults,
        int capacityOfChildren
    )
    {
        Number = number;
        Price = price;
        IsAvailable = isAvailable;
        CapacityOfAdults = capacityOfAdults;
        CapacityOfChildren = capacityOfChildren;
        Type = type;

        DiscountedPrice = price.Amount;
    }

    public static Room Create(
        int number,
        Money price,
        RoomType type,
        int capacityOfAdults = 2,
        int capacityOfChildren = 0,
        bool isAvailable = true
    )
    {
        return new Room(number, price, type, isAvailable, capacityOfAdults, capacityOfChildren);
    }

    public void UpdateDiscountedPrice()
    {
        var discountedPrice = Price;

        foreach (var discount in Discounts)
        {
            discountedPrice = discount.Apply(Price);
        }

        DiscountedPrice = discountedPrice.Amount;
    }

    public Result AddDiscount(Discount discount, DateTime now)
    {
        if (discount.IsApplicable(now))
        {
            return DomainErrors.Discount.NotApplicable;
        }

        if (Discounts.Any(x => x.Name == discount.Name))
        {
            return DomainErrors.Discount.AlreadyExists;
        }

        if (discount.DiscountPercentage is < 0 or > 100)
        {
            return DomainErrors.Discount.InvalidDiscountPercentage;
        }

        if (discount.StartDate > discount.EndDate)
        {
            return DomainErrors.Discount.InvalidDateRange;
        }

        Discounts.Add(discount);

        UpdateDiscountedPrice();

        return Result.Success();
    }

    public int Number { get; private set; }
    public Money Price { get; private set; }
    public decimal DiscountedPrice { get; private set; }
    public RoomType Type { get; private set; }
    public bool IsAvailable { get; private set; }
    public int CapacityOfAdults { get; private set; }
    public int CapacityOfChildren { get; private set; }
    public ICollection<Discount> Discounts { get; private set; } = new List<Discount>();
    public int HotelId { get; private set; }
    public Hotel Hotel { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public ICollection<Booking> Bookings { get; private set; } = new List<Booking>();

    public Result Update(
        int commandNumber,
        Money price,
        RoomType commandType,
        int commandCapacityOfAdults,
        int commandCapacityOfChildren
    )
    {
        if (
            Number == commandNumber
            && Price == price
            && Type == commandType
            && CapacityOfAdults == commandCapacityOfAdults
            && CapacityOfChildren == commandCapacityOfChildren
        )
        {
            return DomainErrors.Room.NothingToUpdate;
        }

        Number = commandNumber;
        Price = price;
        Type = commandType;
        CapacityOfAdults = commandCapacityOfAdults;
        CapacityOfChildren = commandCapacityOfChildren;

        UpdateDiscountedPrice();

        return Result.Success();
    }

    public Result UpdateAvailability(bool isAvailable)
    {
        if (IsAvailable == isAvailable)
        {
            return DomainErrors.Room.NothingToUpdate;
        }

        IsAvailable = isAvailable;

        return Result.Success();
    }

    public int GetDiscountedPercentage()
    {
        var discountedPercentage = Discounts.Aggregate(
            0,
            (current, discount) => current + discount.DiscountPercentage
        );

        return discountedPercentage;
    }
}
