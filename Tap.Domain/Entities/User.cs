using Tap.Domain.Core.Abstraction;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Utility;

namespace Tap.Domain.Entities;

public class User : Entity, IAuditableEntity
{
    public User(string name, string email, string password)
    {
        Ensure.NotEmpty(name, "The first name is required.", nameof(name));
        Ensure.NotEmpty(email, "The email is required.", nameof(email));
        Ensure.NotEmpty(password, "The password is required", nameof(password));

        Name = name;
        Email = email;
        Password = password;
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? UpdatedAtUtc { get; }
}
