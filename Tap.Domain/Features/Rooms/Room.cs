using Tap.Domain.Common.ValueObjects;
using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Primitives;
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

        UpdateDiscountedPrice();
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
            if (discount.IsApplicable())
            {
                discountedPrice = discount.Apply(Price);
            }
        }

        Console.WriteLine(
            $"Room {Number} has a price of {Price.Amount} {Price.Currency} and a discounted price of {discountedPrice.Amount} {discountedPrice.Currency}"
        );

        DiscountedPrice = discountedPrice.Amount;
    }

    public void AddDiscount(Discount discount)
    {
        if (discount is null)
        {
            throw new ArgumentNullException(nameof(discount));
        }

        Discounts.Add(discount);
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

    public void Update(
        int commandNumber,
        Money price,
        RoomType commandType,
        int commandCapacityOfAdults,
        int commandCapacityOfChildren
    )
    {
        Number = commandNumber;
        Price = price;
        Type = commandType;
        CapacityOfAdults = commandCapacityOfAdults;
        CapacityOfChildren = commandCapacityOfChildren;
    }
}
