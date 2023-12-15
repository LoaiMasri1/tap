using MediatR;
using Tap.Domain.Core.Primitives;
using Tap.Domain.Core.Primitives.Result;

namespace Tap.Domain.Core.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error NotFound =>
            new("User.NotFound", "The user with the specified identifier was not found.");

        public static Error DuplicateEmail =>
            new("User.DuplicateEmail", "The specified email address is already in use.");

        public static Error UserAllReadyActive =>
            new("User.UserAllReadyActive", "The user is already active.");

        public static Error ActivationTokenExpired =>
            new("User.ActivationTokenExpired", "The activation token is expired.");
    }

    public static class Email
    {
        public static Error NullOrEmpty => new("Email.NullOrEmpty", "The email is required.");

        public static Error LongerThanAllowed =>
            new("Email.LongerThanAllowed", "The email is longer than allowed.");

        public static Error InvalidFormat =>
            new("Email.InvalidFormat", "The email format is invalid.");
    }

    public static class Password
    {
        public static Error NullOrEmpty => new("Password.NullOrEmpty", "The password is required.");

        public static Error ShorterThanAllowed =>
            new("Password.ShorterThanAllowed", "The password is shorter than allowed.");

        public static Error LongerThanAllowed =>
            new("Password.LongerThanAllowed", "The password is longer than allowed.");

        public static Error MissingLowercase =>
            new(
                "Password.MissingLowercase",
                "The password must contain at least one lowercase letter."
            );

        public static Error MissingUppercase =>
            new(
                "Password.MissingUppercase",
                "The password must contain at least one uppercase letter."
            );

        public static Error MissingDigit =>
            new("Password.MissingDigit", "The password must contain at least one digit.");

        public static Error MissingNonAlphanumeric =>
            new(
                "Password.MissingNonAlphanumeric",
                "The password must contain at least one non-alphanumeric character."
            );
    }

    public static class General
    {
        public static Error UnProcessableRequest =>
            new("General.UnProcessableRequest", "The server could not process the request.");

        public static Error ServerError =>
            new("General.ServerError", "The server encountered an unrecoverable error.");
    }
}
