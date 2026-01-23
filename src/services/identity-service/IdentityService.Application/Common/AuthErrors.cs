namespace IdentityService.Application.Common
{
    public static class AuthErrors
    {
        public static readonly Error LoginFailed = new("User.LoginFailed", "Incorrect email or password");

        public static readonly Error EmailAlreadyExists = new("User.EmailExists", "Email already exists");
        public static readonly Error EmailRequired = new("User.EmailRequired", "Email is required");
        public static readonly Error EmailInvalid = new("User.EmailInvalid", "Invalid email format");
        public static readonly Error EmailMaxLength = new("User.EmailLength", "Email must not exceed 255 characters");

        public static readonly Error PasswordRequired = new("User.PasswordRequired", "Password is required");
        public static readonly Error PasswordMinimum = new("User.PasswordMinimum", "Password must be at least 8 characters");
        public static readonly Error PasswordMaximum = new("User.PasswordMaximum", "Password is too long");
        public static readonly Error PasswordUppercase = new("User.PasswordUppercase", "Password must contain at least one uppercase letter");
        public static readonly Error PasswordLowercase = new("User.PasswordLowercase", "Password must contain at least one lowercase letter");
        public static readonly Error PasswordDigit = new("User.PasswordDigit", "Password must contain at least one digit");
        public static readonly Error PasswordSpecialChar = new("User.PasswordSpecialCharacter", "Password must contain at least one special character");

        public static readonly Error RefreshTokenRequired = new("User.RefreshTokenRequired", "Refresh token is required");
        public static readonly Error RefreshTokenExpired = new("User.RefreshTokenExpired", "The refresh token has expired.");
        public static readonly Error RefreshTokenNotExist = new("User.RefreshTokenNotExist", "The refresh token does not exist");

        public static readonly Error UserIdRequired = new("User.UserRequired", "UserId is required");
        public static readonly Error UserNotExist = new("User.UserNotExist", "This user does not exist or their account has been locked.");

        public static readonly Error PhoneRequired = new("User.PhoneIsRequired", "Phone is required");
        public static readonly Error PhoneInvalid = new("User.PhoneInvalid", "Invalid phone format");

        public static readonly Error UsernameRequired = new("User.UsernameRequired", "Username is required");
        public static readonly Error UsernameMinimum = new("User.UsernameMinimum", "Username must be at least 3 characters");
        public static readonly Error UsernameMaximum = new("User.UsernameMaximum", "Username must not exceed 50 characters");
        public static readonly Error UsernameInvalid = new("User.UsernameInvalid", "Username can only contain letters, numbers and underscores");
    }
}