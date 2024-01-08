using Sieve.Services;
using Tap.Domain.Features.Cities;

namespace Tap.Application.Features.Cities;

public class CitySieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<City>(x => x.Id).CanFilter().CanSort();
        mapper.Property<City>(x => x.Name).CanFilter().CanSort();
        mapper.Property<City>(x => x.Country).CanFilter().CanSort();
        mapper.Property<City>(x => x.Description).CanFilter();
        mapper.Property<City>(x => x.CreatedAtUtc).CanFilter().CanSort().HasName("created");
        mapper.Property<City>(x => x.UpdatedAtUtc).CanFilter().CanSort().HasName("updated");
    }
}
