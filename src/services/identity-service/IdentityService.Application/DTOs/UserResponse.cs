namespace IdentityService.Application.DTOs
{
    public sealed record UserResponse
    (
        string UserId,
        string Email,
        string Phone,
        string UserName,
        bool IsLocked,
        int FailedLoginAttempts,
        bool IsBanned,
        DateTime CreatedAt,
        DateTime? LockedUntil,
        DateTime? BannedUntil
    );
}
