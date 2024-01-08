using Sieve.Services;
using Tap.Domain.Features.Discounts;

namespace Tap.Application.Features.Discounts;

public class DiscountSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Discount>(x => x.Id).CanFilter().CanSort();
        mapper.Property<Discount>(x => x.Name).CanFilter().CanSort();
        mapper.Property<Discount>(x => x.Description).CanFilter();
        mapper
            .Property<Discount>(x => x.DiscountPercentage)
            .CanFilter()
            .CanSort()
            .HasName("discount");
        mapper.Property<Discount>(x => x.StartDate).CanFilter().CanSort();
        mapper.Property<Discount>(x => x.EndDate).CanFilter().CanSort();
        mapper.Property<Discount>(x => x.CreatedAtUtc).CanFilter().CanSort().HasName("created");
        mapper.Property<Discount>(x => x.UpdatedAtUtc).CanFilter().CanSort().HasName("updated");
    }
}
