using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Utility;
using Tap.Domain.Features.Users;

namespace Tap.Domain.Features.Hotels;

public class Hotel : Entity, IAuditableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Location Location { get; private set; }
    public int CityId { get; private set; }
    public int UserId { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? UpdatedAtUtc { get; }

    private Hotel() { }

    public Hotel(string name, string description, Location location, User user)
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(description, "The description is required.", nameof(description));
        Ensure.NotDefault(location, "The location is required.", nameof(location));
        Ensure.NotNull(user, "The user is required.", nameof(user));

        Name = name;
        Description = description;
        Location = location;
        UserId = user.Id;
    }
}
