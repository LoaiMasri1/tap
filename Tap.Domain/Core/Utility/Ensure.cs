using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Exceptions;

namespace Tap.Domain.Core.Utility;

public static class Ensure
{
    public static void NotEmpty(string value, string? message, string? argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty(Guid value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";

        if (value == Guid.Empty)
        {
            throw new DomainException(DomainErrors.General.NotEmpty.WithMessage(error));
        }
    }

    public static void NotEmpty(DateTime value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";
        if (value == default)
        {
            throw new DomainException(DomainErrors.General.NotEmpty.WithMessage(error));
        }
    }

    public static void NotEmpty(int value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";
        if (value == default)
        {
            throw new DomainException(DomainErrors.General.NotEmpty.WithMessage(error));
        }
    }

    public static void NotNull<TValue>(TValue value, string? message, string? argumentName)
        where TValue : class
    {
        var error = $"{argumentName} {message}";
        if (value is null)
        {
            throw new DomainException(DomainErrors.General.NotNull.WithMessage(error));
        }
    }

    public static void NotDefault<TValue>(TValue value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";

        if (EqualityComparer<TValue>.Default.Equals(value, default))
        {
            throw new DomainException(DomainErrors.General.NotDefault.WithMessage(error));
        }
    }

    public static void NotFuture(DateTime value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";

        if (value > DateTime.UtcNow)
        {
            throw new DomainException(DomainErrors.General.NotFuture.WithMessage(error));
        }
    }

    public static void NotPast(DateTime value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";
        if (value < DateTime.UtcNow)
        {
            throw new DomainException(DomainErrors.General.NotPast.WithMessage(error));
        }
    }

    public static void Positive(int value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";
        if (value <= 0)
        {
            throw new DomainException(DomainErrors.General.Positive.WithMessage(error));
        }
    }

    public static void Of<TEnum>(TEnum value, string? message, string? argumentName)
        where TEnum : struct, Enum
    {
        var error = $"{argumentName} {message}";
        if (!Enum.IsDefined(typeof(TEnum), value))
        {
            throw new DomainException(DomainErrors.General.Of.WithMessage(error));
        }
    }

    public static void LessThan<TValue>(
        TValue value,
        TValue max,
        string? message,
        string? argumentName
    )
        where TValue : struct, IComparable<TValue>
    {
        var error = $"{argumentName} {message}";
        if (value.CompareTo(max) > 0)
        {
            throw new DomainException(DomainErrors.General.LessThan.WithMessage(error));
        }
    }

    public static void GreaterThan<TValue>(
        TValue value,
        TValue min,
        string? message,
        string? argumentName
    )
        where TValue : struct, IComparable<TValue>
    {
        var error = $"{argumentName} {message}";
        if (value.CompareTo(min) < 0)
        {
            throw new DomainException(DomainErrors.General.GreaterThan.WithMessage(error));
        }
    }
}
