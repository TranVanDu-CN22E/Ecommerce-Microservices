namespace IdentityService.Application.Common
{
    public sealed record Error(string Code, string Message);
    public static class UserErrors
    {
        public static readonly Error EmailAlreadyExists =
            new("User.EmailExists", "Email already exists");
    }
}
