namespace Tap.Domain.Core.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }

    public bool Equals(ValueObject? other) =>
        other is not null && GetAtomicValues().SequenceEqual(other.GetAtomicValues());

    protected abstract IEnumerable<object> GetAtomicValues();

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        if (obj is not ValueObject valueObject)
        {
            return false;
        }

        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }

    public override int GetHashCode()
    {
        HashCode hash = default;

        foreach (object value in GetAtomicValues())
        {
            hash.Add(value);
        }

        return hash.ToHashCode();
    }
}
