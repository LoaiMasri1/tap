using Tap.Domain.Core.Primitives;

namespace Tap.Domain.Core.Errors;

public static class DomainsError
{
    public static class User
    {
        public static Error NotFound =>
            new("User.NotFound", "The user with the specified identifier was not found.");
    }
}
