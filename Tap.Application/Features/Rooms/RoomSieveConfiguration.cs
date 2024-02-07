using Sieve.Services;
using Tap.Domain.Features.Rooms;

namespace Tap.Application.Features.Rooms;

public class RoomSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Room>(x => x.Id).CanFilter().CanSort();
        mapper.Property<Room>(x => x.HotelId).CanFilter();
        mapper.Property<Room>(x => x.DiscountedPrice).CanFilter().CanSort();
        mapper.Property<Room>(x => x.Price.Amount).CanFilter().CanSort().HasName("price");
        mapper.Property<Room>(x => x.IsAvailable).CanFilter().CanSort();
        mapper.Property<Room>(x => x.Number).CanFilter().CanSort();
        mapper.Property<Room>(x => x.Type).CanFilter();
        mapper.Property<Room>(x => x.CapacityOfAdults).CanFilter().CanSort().HasName("adults");
        mapper.Property<Room>(x => x.CapacityOfChildren).CanFilter().CanSort().HasName("children");
        mapper.Property<Room>(x => x.CreatedAtUtc).CanFilter().CanSort().HasName("created");
        mapper.Property<Room>(x => x.UpdatedAtUtc).CanFilter().CanSort().HasName("updated");
    }
}
