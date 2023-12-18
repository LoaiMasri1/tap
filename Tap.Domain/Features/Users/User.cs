using Tap.Domain.Common.Services;
using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Errors;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;
using Tap.Domain.Core.Utility;
using Tap.Domain.Features.Hotels;

namespace Tap.Domain.Features.Users;

public class User : Entity, IAuditableEntity
{
    private string _hashedPassword;

    private User() { }

    public User(
        string name,
        Email email,
        string hashedPassword,
        UserRole role,
        Token? activationToken = null
    )
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(email, "The email is required.", nameof(email));
        Ensure.NotEmpty(hashedPassword, "The password is required", nameof(hashedPassword));
        Ensure.NotDefault(role, "The role is required", nameof(role));

        Name = name;
        Email = email;
        _hashedPassword = hashedPassword;
        Role = role;
        ActivationToken = activationToken;
    }

    public string Name { get; private set; }
    public Email Email { get; private set; }
    public UserRole Role { get; private set; }
    public Token? ActivationToken { get; private set; }
    public ICollection<Hotel> Hotels { get; private set; } = new List<Hotel>();
    public bool IsActivate { get; private set; } = false;
    public DateTime CreatedAtUtc { get; }
    public DateTime? UpdatedAtUtc { get; }

    public void Activate() => IsActivate = true;

    public bool VerifyPasswordHash(string password, IPasswordHashChecker passwordHashChecker) =>
        !string.IsNullOrWhiteSpace(password)
        && passwordHashChecker.HashesMatch(_hashedPassword, password);

    public void UpdateName(string firstName, string lastName)
    {
        Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));

        Name = $"{firstName} {lastName}";
    }

    public Result UpdatePassword(string hashedPassword)
    {
        Ensure.NotEmpty(hashedPassword, "The password is required.", nameof(hashedPassword));

        if (_hashedPassword == hashedPassword)
            return DomainErrors.User.CannotChangePassword;

        _hashedPassword = hashedPassword;

        return Result.Success();
    }
}
