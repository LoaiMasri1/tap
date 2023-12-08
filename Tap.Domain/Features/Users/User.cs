using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Utility;

namespace Tap.Domain.Features.Users;

public class User : Entity, IAuditableEntity
{
    private string _hashedPassword;

    private User() { }

    public User(string name, Email email, string hashedPassword, UserRole role)
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(email, "The email is required.", nameof(email));
        Ensure.NotEmpty(hashedPassword, "The password is required", nameof(hashedPassword));
        Ensure.NotDefault(role, "The role is required", nameof(role));

        Name = name;
        Email = email;
        _hashedPassword = hashedPassword;
        Role = role;
    }

    public string Name { get; private set; }
    public Email Email { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? UpdatedAtUtc { get; }
}
