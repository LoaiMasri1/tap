namespace Tap.Domain.Core.Utility;

public static class Ensure
{
    public static void NotEmpty(string value, string message, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty(Guid value, string message, string argumentName)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty(DateTime value, string message, string argumentName)
    {
        if (value == default)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty(int value, string message, string argumentName)
    {
        if (value == default)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotNull<TValue>(TValue value, string message, string argumentName)
        where TValue : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(argumentName, message);
        }
    }

    public static void NotDefault<TValue>(TValue value, string message, string argumentName)
    {
        if (EqualityComparer<TValue>.Default.Equals(value, default))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotFuture(DateTime value, string message, string argumentName)
    {
        if (value > DateTime.UtcNow)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotPast(DateTime value, string message, string argumentName)
    {
        if (value < DateTime.UtcNow)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void Positive(int value, string message, string argumentName)
    {
        if (value <= 0)
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void Of<TEnum>(TEnum value, string message, string argumentName)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(typeof(TEnum), value))
        {
            throw new ArgumentException(message, argumentName);
        }
    }
}
