namespace IdentityService.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string email, string userName, CancellationToken ct);
        Task SendAccountLockedEmailAsync(string email, string userName, string reason, DateTime? lockedUntil, CancellationToken ct);
        Task SendAccountOtpEmailAsync(string email, string otp, CancellationToken ct);
    }
}
