using Sieve.Services;
using Tap.Domain.Features.Amenities;

namespace Tap.Application.Features.Amenities;

public class AmenitySieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Amenity>(x => x.Id).CanFilter().CanSort();
        mapper.Property<Amenity>(x => x.Name).CanFilter().CanSort();
        mapper.Property<Amenity>(x => x.Description).CanFilter();
        mapper.Property<Amenity>(x => x.CreatedAtUtc).CanFilter().CanSort().HasName("created");
        mapper.Property<Amenity>(x => x.UpdatedAtUtc).CanFilter().CanSort().HasName("updated");
    }
}
