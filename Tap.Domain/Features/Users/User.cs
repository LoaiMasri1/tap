using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Utility;

namespace Tap.Domain.Features.Users;

public class User : Entity, IAuditableEntity
{
    private User() { }

    public User(string name, Email email, Password password, UserRole role)
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(email, "The email is required.", nameof(email));
        Ensure.NotEmpty(password, "The password is required", nameof(password));
        Ensure.NotDefault(role, "The role is required", nameof(role));

        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? UpdatedAtUtc { get; }
}
