namespace Tap.Services.Api.Contracts;

public static class ApiRoutes
{
    public static class User
    {
        private const string Base = "users";
        public const string Get = Base;
        public const string Post = Base;
        public const string Update = $"{Base}/{{id}}";

        public const string Activate = $"{Base}/activate";
    }

    public static class Auth
    {
        private const string Base = "auth";
        public const string Login = $"{Base}/login";
    }

    public static class City
    {
        private const string Base = "cities";
        public const string Get = Base;
        public const string Post = Base;
        public const string CreateHotel = $"{Base}/{{id}}/hotels";
        public const string UploadPhotos = $"{Base}/{{id}}/photos";
    }

    public static class Hotel
    {
        private const string Base = "hotels";
        public const string Get = Base;
        public const string Post = Base;
        public const string UploadPhotos = $"{Base}/{{id}}/photos";
    }

    public static class Room
    {
        private const string Base = "rooms";
        public const string Get = Base;
        public const string Post = Base;
        public const string UploadPhotos = $"{Base}/{{id}}/photos";
    }

    // upload photos
    public static class Photo
    {
        private const string Base = "photos";
        public const string Get = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
        public const string Upload = $"{Base}/items/{{id}}/type/{{type}}";
    }
}
