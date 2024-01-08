using Sieve.Services;
using Tap.Domain.Features.Hotels;

namespace Tap.Application.Features.Hotels;

public class HotelSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Hotel>(x => x.Id).CanFilter().CanSort();
        mapper.Property<Hotel>(x => x.Name).CanFilter().CanSort();
        mapper.Property<Hotel>(x => x.Description).CanFilter();
        mapper.Property<Hotel>(x => x.CityId).CanFilter();
        mapper.Property<Hotel>(x => x.City.Name).CanFilter().CanSort();
        mapper.Property<Hotel>(x => x.Rating).CanFilter().CanSort();
        mapper.Property<Hotel>(x => x.UserId).CanFilter().HasName("ownerId"); // TODO: Fix this in the future. This is a workaround for the client-side.
        mapper.Property<Hotel>(x => x.Location.Latitude).CanFilter().CanSort().HasName("latitude");
        mapper.Property<Hotel>(x => x.Location.Latitude).CanFilter().CanSort().HasName("longitude");
        mapper.Property<Hotel>(x => x.CreatedAtUtc).CanFilter().CanSort().HasName("created");
        mapper.Property<Hotel>(x => x.UpdatedAtUtc).CanFilter().CanSort().HasName("updated");
    }
}
