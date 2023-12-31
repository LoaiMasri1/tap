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

    public class CreateDiscount
    {
        public static Error NameIsRequired =>
            new("CreateDiscount.NameIsRequired", "The name is required.");

        public static Error DescriptionIsRequired =>
            new("CreateDiscount.DescriptionIsRequired", "The description is required.");

        public static Error DiscountPercentageIsRequired =>
            new(
                "CreateDiscount.DiscountPercentageIsRequired",
                "The discount percentage is required."
            );

        public static Error StartDateIsRequired =>
            new("CreateDiscount.StartDateIsRequired", "The start date is required.");

        public static Error EndDateIsRequired =>
            new("CreateDiscount.EndDateIsRequired", "The end date is required.");

        public static Error StartDateMustBeGreaterThanOrEqualToToday =>
            new(
                "CreateDiscount.StartDateMustBeGreaterThanOrEqualToToday",
                "The start date must be greater than or equal to today."
            );

        public static Error EndDateMustBeGreaterThanOrEqualToToday =>
            new(
                "CreateDiscount.EndDateMustBeGreaterThanOrEqualToToday",
                "The end date must be greater than or equal to today."
            );

        public static Error EndDateMustBeGreaterThanOrEqualToStartDate =
            new(
                "CreateDiscount.EndDateMustBeGreaterThanOrEqualToStartDate",
                "The end date must be greater than or equal to start date."
            );
    }

    public class CreateRoom
    {
        public static Error PriceIsRequired =>
            new("CreateRoom.PriceIsRequired", "The price is required.");

        public static Error HotelIdIsRequired =>
            new("CreateRoom.HotelIdIsRequired", "The hotel id is required.");

        public static Error CurrencyIsRequired =>
            new("CreateRoom.CurrencyIsRequired", "The currency is required.");

        public static Error TypeIsRequired =>
            new("CreateRoom.TypeIsRequired", "The type is required.");
        public static Error TypeIsInvalid =>
            new("CreateRoom.TypeIsInvalid", "The type is invalid.");
        public static Error CapacityOfAdultsIsRequired =>
            new("CreateRoom.CapacityOfAdultsIsRequired", "The capacity of adults is required.");
        public static Error CapacityOfChildrenIsRequired =>
            new("CreateRoom.CapacityOfChildrenIsRequired", "The capacity of children is required.");

        public static Error NumberIsRequired =>
            new("CreateRoom.NumberIsRequired", "The number is required.");
    }

    public class UpdateRoom
    {
        public static Error IdIsRequired => new("UpdateRoom.IdIsRequired", "The id is required.");
        public static Error NumberIsRequired =>
            new("UpdateRoom.NumberIsRequired", "The number is required.");
        public static Error PriceIsRequired =>
            new("UpdateRoom.PriceIsRequired", "The price is required.");
        public static Error CurrencyIsRequired =>
            new("UpdateRoom.CurrencyIsRequired", "The currency is required.");
        public static Error TypeIsRequired =>
            new("UpdateRoom.TypeIsRequired", "The type is required.");
        public static Error CapacityOfAdultsIsRequired =>
            new("UpdateRoom.CapacityOfAdultsIsRequired", "The capacity of adults is required.");
        public static Error CapacityOfChildrenIsRequired =>
            new("UpdateRoom.CapacityOfChildrenIsRequired", "The capacity of children is required.");
    }

    public class DeleteRoom
    {
        public static Error RoomIdIsRequired =>
            new("DeleteRoom.RoomIdIsRequired", "The room id is required.");
    }

    public class CreateReview
    {
        public static Error TitleIsRequired =>
            new("CreateReview.TitleIsRequired", "The title is required.");
        public static Error ContentIsRequired =>
            new("CreateReview.ContentIsRequired", "The content is required.");
        public static Error RatingIsRequired =>
            new("CreateReview.RatingIsRequired", "The rating is required.");
        public static Error RatingIsInvalid =>
            new("CreateReview.RatingIsInvalid", "The rating is invalid.");
        public static Error HotelIdIsRequired =>
            new("CreateReview.HotelIdIsRequired", "The hotel id is required.");
        public static Error UserIdIsRequired =>
            new("CreateReview.UserIdIsRequired", "The user id is required.");
    }

    public class DeleteReview
    {
        public static Error ReviewIdIsRequired =>
            new("DeleteReview.ReviewIdIsRequired", "The review id is required.");
    }

    public class UpdateReview
    {
        public static Error ReviewIdIsRequired =>
            new("UpdateReview.ReviewIdIsRequired", "The review id is required.");
        public static Error TitleIsRequired =>
            new("UpdateReview.TitleIsRequired", "The title is required.");
        public static Error ContentIsRequired =>
            new("UpdateReview.ContentIsRequired", "The content is required.");
        public static Error RatingIsRequired =>
            new("UpdateReview.RatingIsRequired", "The rating is required.");
    }
}
