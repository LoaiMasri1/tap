using Tap.Domain.Core.Primitives;

namespace Tap.Application.Core.Errors;

public static class ValidationErrors
{
    internal static class CreateUser
    {
        internal static Error NameIsRequired =>
            new("CreateUser.NameIsRequired", "The name is required.");

        internal static Error EmailIsRequired =>
            new("CreateUser.EmailIsRequired", "The email is required.");

        internal static Error PasswordIsRequired =>
            new("CreateUser.PasswordIsRequired", "The password is required.");
        internal static Error RoleIsRequired =>
            new("CreateUser.RoleIsRequired", "The role is required.");
    }
}
