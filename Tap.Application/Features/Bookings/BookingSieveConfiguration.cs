using Sieve.Services;
using Tap.Domain.Features.Bookings;

namespace Tap.Application.Features.Bookings;

public class BookingSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Booking>(x => x.Id).CanFilter().CanSort();
        mapper.Property<Booking>(x => x.HotelId).CanFilter();
        mapper.Property<Booking>(x => x.UserId).CanFilter();
        mapper.Property<Booking>(x => x.Status).CanFilter().CanSort();
        mapper.Property<Booking>(x => x.CheckInDate).CanFilter().CanSort().HasName("checkIn");
        mapper.Property<Booking>(x => x.CheckOutDate).CanFilter().CanSort().HasName("checkOut");
        mapper.Property<Booking>(x => x.CreatedAtUtc).CanFilter().CanSort().HasName("created");
        mapper.Property<Booking>(x => x.UpdatedAtUtc).CanFilter().CanSort().HasName("updated");
    }
}
