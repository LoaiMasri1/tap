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
}
