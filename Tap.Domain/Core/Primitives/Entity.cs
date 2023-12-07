using Tap.Domain.Core.Utility;

namespace Tap.Domain.Core.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity(int id)
        : this()
    {
        Ensure.NotEmpty(id, "The identifier is required.", nameof(id));

        Id = id;
    }

    protected Entity() { }

    public int Id { get; private set; }

    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        if (Id == default || other.Id == default)
        {
            return false;
        }

        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        return Equals(obj as Entity);
    }

    public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
}
