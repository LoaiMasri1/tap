using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Core.Utility;

namespace Tap.Domain.Features.Users;

public sealed class Password : ValueObject
{
    private const int MinLength = 8;
    public const int MaxLength = 128;

    private Password(string value) => Value = value;

    public string Value { get; }

    public static Password From(string value)
    {
        Ensure.NotEmpty(value, "The password is required.", nameof(value));

        return new Password(value);
    }

    public static Result<Password> Create(string password) =>
        Result
            .Create(password)
            .Ensure(p => !string.IsNullOrWhiteSpace(p), DomainErrors.Password.NullOrEmpty)
            .Ensure(p => p.Length >= MinLength, DomainErrors.Password.ShorterThanAllowed)
            .Ensure(p => p.Length <= MaxLength, DomainErrors.Password.LongerThanAllowed)
            .Ensure(p => p.Any(char.IsLower), DomainErrors.Password.MissingLowercase)
            .Ensure(p => p.Any(char.IsUpper), DomainErrors.Password.MissingUppercase)
            .Ensure(p => p.Any(char.IsDigit), DomainErrors.Password.MissingDigit)
            .Ensure(
                p => p.Any(e => !char.IsLetterOrDigit(e)),
                DomainErrors.Password.MissingNonAlphanumeric
            )
            .Map(p => new Password(p));

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator string(Password password) => password.Value;

    public static explicit operator Password(string password) => From(password);
}
