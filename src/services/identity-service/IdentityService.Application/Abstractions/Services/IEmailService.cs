namespace IdentityService.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string email, string userName, CancellationToken ct);
        Task SendAccountLockedEmailAsync(string email, string reason, DateTime? lockedUntil, CancellationToken ct);
    }
}
