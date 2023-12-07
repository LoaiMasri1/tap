using Tap.Domain.Core.Primitives;

namespace Tap.Domain.Core.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error NotFound =>
            new("User.NotFound", "The user with the specified identifier was not found.");

        public static Error DuplicateEmail =>
            new("User.DuplicateEmail", "The specified email address is already in use.");
    }

    public static class General
    {
        public static Error UnProcessableRequest =>
            new Error("General.UnProcessableRequest", "The server could not process the request.");

        public static Error ServerError =>
            new Error("General.ServerError", "The server encountered an unrecoverable error.");
    }
}
