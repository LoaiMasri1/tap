using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Utility;
using Tap.Domain.Features.Hotels;

namespace Tap.Domain.Features.Cities;

public class City : Entity, IAuditableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Country { get; private set; }
    public ICollection<Hotel> Hotels { get; private set; } = new List<Hotel>();
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    private City() { }

    public City(string name, string description, string country)
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(description, "The description is required.", nameof(description));
        Ensure.NotEmpty(country, "The country is required.", nameof(country));

        Name = name;
        Description = description;
        Country = country;
    }

    public void AddHotel(Hotel hotel)
    {
        Ensure.NotNull(hotel, "The hotel is required.", nameof(hotel));

        Hotels.Add(hotel);
    }
}
