using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Core.Utility;

namespace Tap.Domain.Features.Amenities;

public class Amenity : Entity, IAuditableEntity
{
    private Amenity() { }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public AmenityType Type { get; private set; }
    public int TypeId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    private Amenity(string name, string description, AmenityType type, int typeId)
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(description, "The description is required.", nameof(description));
        Ensure.Of(type, "The type should be one of the following: Hotel, Room", nameof(type));
        Ensure.NotDefault(typeId, "The type id is required.", nameof(typeId));

        Name = name;
        Description = description;
        Type = type;
        TypeId = typeId;
    }

    public static Amenity Create(string name, string description, AmenityType type, int typeId) =>
        new(name, description, type, typeId);

    public Result Update(string name, string description)
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(description, "The description is required.", nameof(description));

        if (Name == name && Description == description)
        {
            return DomainErrors.Amenity.NothingToUpdate;
        }

        Name = name;
        Description = description;

        return Result.Success();
    }
}
