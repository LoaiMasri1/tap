﻿using Tap.Domain.Core.Primitives;
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

    public static class Photo
    {
        public static Error UrlNullOrEmpty =>
            new("Photo.UrlNullOrEmpty", "The photo url is required.");
        public static Error TypeInvalid => new("Photo.TypeInvalid", "The photo type is invalid.");
        public static Error ItemIdInvalid =>
            new("Photo.ItemIdInvalid", "The photo item id is invalid.");
        public static Error EnsurePhotosCount =>
            new("Photo.EnsurePhotosCount", "The photos count must be between 1 and 5.");
        public static Error EnsurePhotosType =>
            new("Photo.EnsurePhotosType", "The photos type must be jpeg or png.");
        public static Error EnsurePhotosSize =>
            new("Photo.EnsurePhotosSize", "The photos size must be less than 5MB.");

        public static Error PhotoNotFound =>
            new("Photo.PhotoNotFound", "The photo with the specified identifier was not found.");
    }

    public static class General
    {
        public static Error UnProcessableRequest =>
            new("General.UnProcessableRequest", "The server could not process the request.");

        public static Error ServerError =>
            new("General.ServerError", "The server encountered an unrecoverable error.");
    }

    public class Hotel
    {
        public static Error NotFound =>
            new("Hotel.NotFound", "The hotel with the specified identifier was not found.");

        public static Error NothingToUpdate =>
            new("Hotel.NothingToUpdate", "Nothing to update. The hotel is already up to date.");
        public static Error AmenityTypeShouldBeHotel =>
            new("Hotel.AmenityTypeShouldBeHotel", "The amenity type should be Hotel.");
    }

    public class Amenity
    {
        public static Error InvalidType =>
            new("Amenity.InvalidType", "The amenity type is invalid.");

        public static Error NameIsRequired =>
            new("Amenity.NameIsRequired", "The amenity name is required.");

        public static Error DescriptionIsRequired =>
            new("Amenity.DescriptionIsRequired", "The amenity description is required.");
        public static Error TypeShouldBeOneOfTheFollowingHotelRoom =>
            new(
                "Amenity.TypeShouldBeOneOfTheFollowingHotelRoom",
                "The amenity type should be one of the following: Hotel, Room"
            );

        public static Error TypeIdIsRequired =>
            new("Amenity.TypeIdIsRequired", "The amenity type id is required.");

        public static Error NothingToUpdate =>
            new("Amenity.NothingToUpdate", "Nothing to update. The amenity is already up to date.");

        public static Error NotFound =>
            new("Amenity.NotFound", "The amenity with the specified identifier was not found.");
    }

    public class Room
    {
        public static Error AmenityTypeShouldBeRoom =>
            new("Room.AmenityTypeShouldBeRoom", "The amenity type should be Room.");

        public static Error NotFound =>
            new("Room.NotFound", "The room with the specified identifier was not found.");
        public static Error NothingToUpdate =>
            new("Room.NothingToUpdate", "Nothing to update. The room is already up to date.");
    }

    public class Discount
    {
        public static Error NotApplicable =>
            new("Discount.NotApplicable", "The discount is not applicable.");

        public static Error AlreadyExists =>
            new("Discount.AlreadyExists", "The discount with the specified name already exists.");

        public static Error InvalidDiscountPercentage =>
            new(
                "Discount.InvalidDiscountPercentage",
                "The discount percentage should be greater than 0 and less than 100."
            );
        public static Error InvalidDateRange =>
            new("Discount.InvalidDateRange", "The start date should be less than the end date.");
    }
}
