namespace Tap.Services.Api.Contracts;

public static class ApiRoutes
{
    public static class User
    {
        private const string Base = "users";
        public const string Get = Base;
        public const string Update = $"{Base}/{{id}}";
    }

    public static class Auth
    {
        private const string Base = "auth";
        public const string Login = $"{Base}/login";
        public const string Register = $"{Base}/register";
        public const string Activate = $"{Base}/activate";
    }

    public static class City
    {
        private const string Base = "cities";
        public const string Get = Base;
        public const string GetById = $"{Base}/{{id}}";
        public const string Post = Base;
        public const string CreateHotel = $"{Base}/{{id}}/hotels";
        public const string UploadPhotos = $"{Base}/{{id}}/photos";
    }

    public static class Hotel
    {
        private const string Base = "hotels";
        public const string Get = Base;
        public const string GetById = $"{Base}/{{id}}";
        public const string Post = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string UploadPhotos = $"{Base}/{{id}}/photos";
        public const string CreateAmenities = $"{Base}/{{id}}/amenities";
        public const string CreateRoom = $"{Base}/{{id}}/rooms";
    }

    public static class Discount
    {
        private const string Base = "discounts";
        public const string GetByHotelId = $"{Base}/hotels/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }

    public static class Room
    {
        private const string Base = "rooms";
        public const string Get = Base;
        public const string GeByHotelId = $"{Base}/hotels/{{id}}";
        public const string Post = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
        public const string UploadPhotos = $"{Base}/{{id}}/photos";
        public const string AddAmenities = $"{Base}/{{id}}/amenities";
        public const string AddDiscount = $"{Base}/{{id}}/discounts";
        public const string Book = $"{Base}/{{id}}/bookings";
    }

    public static class Photo
    {
        private const string Base = "photos";
        public const string Get = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
        public const string Upload = $"{Base}/items/{{id}}/type/{{type}}";
    }

    public static class Amenity
    {
        private const string Base = "amenities";
        public const string Get = Base;
        public const string Post = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }

    public static class Review
    {
        private const string Base = "reviews";
        public const string Get = Base;
        public const string Post = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }

    public static class Booking
    {
        private const string Base = "bookings";
        public const string Confirm = $"{Base}/{{id}}/confirm";
        public const string Cancel = $"{Base}/{{id}}/cancel";
        public const string Checkout = $"{Base}/{{id}}/checkout";
    }

    public static class Payment
    {
        private const string Base = "payments";
        public const string Success = $"{Base}/success";
        public const string Cancel = $"{Base}/cancel";
    }
}
