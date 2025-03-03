namespace Shared.Routing
{
    public class AuthControllerRoutes
    {
        public const string Root = "api/auth";
        public const string Register = "register";
        public const string Login = "login";
        public const string RefreshToken = "refresh-token";

        public static string RefreshTokenOnClient => $"{Root}/{RefreshToken}";
        public static string LoginOnClient => $"{Root}/{Login}";
        public static string RegisterOnClient = $"{Root}/{Register}";
    }
}
