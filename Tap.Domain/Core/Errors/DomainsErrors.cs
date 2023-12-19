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
        public static Error UserAllReadyActive =>
            new("User.UserAllReadyActive", "The user is already active.");
        public static Error ActivationTokenExpired =>
            new("User.ActivationTokenExpired", "The activation token is expired.");
        public static Error PasswordsDoNotMatch =>
            new("User.PasswordsDoNotMatch", "The passwords do not match.");
        public static Error CannotChangePassword =>
            new("User.CannotChangePassword", "The user cannot change the password.");
        public static Error Unauthorized =>
            new("User.Unauthorized", "The user is not authorized to perform this action.");
    }

    public static class City
    {
        public static Error NotFound =>
            new("City.NotFound", "The city with the specified identifier was not found.");
        public static Error AlreadyExists =>
            new("City.AlreadyExists", "The city with the specified name already exists.");
    }

    public static class Email
    {
        public static Error NullOrEmpty => new("Email.NullOrEmpty", "The email is required.");

        public static Error LongerThanAllowed =>
            new("Email.LongerThanAllowed", "The email is longer than allowed.");

        public static Error InvalidFormat =>
            new("Email.InvalidFormat", "The email format is invalid.");
    }

    public static class Location
    {
        public static Error LatitudeOutOfRange =>
            new("Location.LatitudeOutOfRange", "The latitude is out of range of -90 and 90.");
        public static Error LongitudeOutOfRange =>
            new("Location.LongitudeOutOfRange", "The longitude is out of range. -180 and 180.");
        public static Error NullOrEmpty => new("Location.NullOrEmpty", "The location is required.");
        public static Error NullLatitude =>
            new("Location.NullLatitude", "The latitude is required.");
        public static Error NullLongitude =>
            new("Location.NullLongitude", "The longitude is required.");
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

    public static class Authentication
    {
        public static Error InvalidEmailOrPassword =>
            new("Authentication.InvalidEmailOrPassword", "Invalid email or password.");

        public static Error AccountNotActive =>
            new("Authentication.AccountNotActive", "The account is not active.");
    }

    public static class General
    {
        public static Error UnProcessableRequest =>
            new("General.UnProcessableRequest", "The server could not process the request.");

        public static Error ServerError =>
            new("General.ServerError", "The server encountered an unrecoverable error.");
    }
}
