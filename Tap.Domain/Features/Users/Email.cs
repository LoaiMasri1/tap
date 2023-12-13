using System.Text.RegularExpressions;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Core.Utility;

namespace Tap.Domain.Features.Users;

public sealed class Email : ValueObject
{
    private const string EmailRegexPattern =
        @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

    public const int MaxLength = 254;

    private static readonly Lazy<Regex> _emailRegex =
        new(() => new Regex(EmailRegexPattern, RegexOptions.Compiled));

    private Email(string value) => Value = value;

    public string Value { get; set; }

    public static Result<Email> Create(string email) =>
        Result
            .Create(email)
            .Ensure(e => !string.IsNullOrWhiteSpace(e), DomainErrors.Email.NullOrEmpty)
            .Ensure(e => e.Length <= MaxLength, DomainErrors.Email.LongerThanAllowed)
            .Ensure(e => _emailRegex.Value.IsMatch(e), DomainErrors.Email.InvalidFormat)
            .Map(e => new Email(e));

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Email From(string value)
    {
        Ensure.NotEmpty(value, "The email is required.", nameof(value));

        return new Email(value);
    }

    public static implicit operator string(Email email) => email.Value;

    public static explicit operator Email(string email) => From(email);
}
