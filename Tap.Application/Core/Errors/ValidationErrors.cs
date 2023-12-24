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

    internal static class ActivateUser
    {
        internal static Error TokenIsRequired =>
            new("ActivateUser.TokenIsRequired", "The token is required.");
    }

    internal class Login
    {
        internal static Error EmailIsRequired =>
            new("Login.EmailIsRequired", "The email is required.");

        internal static Error PasswordIsRequired =>
            new("Login.PasswordIsRequired", "The password is required.");
    }

    internal class UpdateUser
    {
        internal static Error IdIsRequired => new("UpdateUser.IdIsRequired", "The id is required.");
        internal static Error FirstNameIsRequired =>
            new("UpdateUser.NameIsRequired", "The first name is required.");
        internal static Error LastNameIsRequired =>
            new("UpdateUser.NameIsRequired", "The last name is required.");
    }

    internal class CreateHotel
    {
        internal static Error NameIsRequired =>
            new("CreateHotel.NameIsRequired", "The name is required.");
        internal static Error DescriptionIsRequired =>
            new("CreateHotel.DescriptionIsRequired", "The description is required.");
        internal static Error LatitudeIsRequired =>
            new("CreateHotel.LatitudeIsRequired", "The latitude is required.");
        internal static Error LongitudeIsRequired =>
            new("CreateHotel.LongitudeIsRequired", "The longitude is required.");
        internal static Error CityIdIsRequired =>
            new("CreateHotel.CityIdIsRequired", "The city id is required.");
    }

    internal class CreateCity
    {
        internal static Error NameIsRequired =>
            new("CreateCity.NameIsRequired", "The name is required.");
        internal static Error DescriptionIsRequired =>
            new("CreateCity.DescriptionIsRequired", "The description is required.");
        internal static Error CountryIsRequired =>
            new("CreateCity.CountryIsRequired", "The country is required.");
        public static Error NameTooLong => new("CreateCity.NameTooLong", "The name is too long.");
        public static Error DescriptionTooLong =>
            new("CreateCity.DescriptionTooLong", "The description is too long.");
        public static Error CountryTooLong =>
            new("CreateCity.CountryTooLong", "The country is too long.");
    }

    public static class UploadPhotos
    {
        public static Error ItemIdRequired =>
            new("UploadPhotos.ItemIdRequired", "The item id is required.");
        public static Error FilesRequired =>
            new("UploadPhotos.FilesRequired", "The files are required.");
    }

    public class UpdatePhoto
    {
        public static Error PhotoIdRequired =>
            new("UpdatePhoto.PhotoIdRequired", "The photo id is required.");
        public static Error FileRequired =>
            new("UpdatePhoto.FileRequired", "The file is required.");
    }

    public class DeletePhoto
    {
        public static Error PhotoIdRequired =>
            new("DeletePhoto.PhotoIdRequired", "The photo id is required.");
    }

    public class UpdateHotel
    {
        public static Error HotelIdRequired =>
            new("UpdateHotel.HotelIdRequired", "The hotel id is required.");

        public static Error NameIsRequired =>
            new("UpdateHotel.NameIsRequired", "The name is required.");

        public static Error DescriptionIsRequired =>
            new("UpdateHotel.DescriptionIsRequired", "The description is required.");

        public static Error LatitudeIsRequired =>
            new("UpdateHotel.LatitudeIsRequired", "The latitude is required.");

        public static Error LongitudeIsRequired =>
            new("UpdateHotel.LongitudeIsRequired", "The longitude is required.");
    }

    public class CreateAmenity { }

    public class UpdateAmenity
    {
        public static Error AmenityIdRequired =>
            new("UpdateAmenity.AmenityIdRequired", "The amenity id is required.");

        public static Error NameIsRequired =>
            new("UpdateAmenity.NameIsRequired", "The name is required.");

        public static Error DescriptionIsRequired =>
            new("UpdateAmenity.DescriptionIsRequired", "The description is required.");
    }

    public class DeleteAmenity
    {
        public static Error AmenityIdRequired =>
            new("DeleteAmenity.AmenityIdRequired", "The amenity id is required.");
    }
}
