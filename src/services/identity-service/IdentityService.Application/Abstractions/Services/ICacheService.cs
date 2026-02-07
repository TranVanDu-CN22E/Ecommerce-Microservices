namespace IdentityService.Application.Abstractions.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
        Task SetAsync<T>(string key, T value, TimeSpan? expire = null, CancellationToken ct = default);
        Task RemoveAsync(string key, CancellationToken ct = default);
    }
}
