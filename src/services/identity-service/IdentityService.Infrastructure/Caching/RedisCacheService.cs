using Microsoft.Extensions.Caching.Distributed;
using IdentityService.Application.Abstractions;
using System.Text.Json;

namespace IdentityService.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            var cached = await _cache.GetStringAsync(key, ct);
            return cached is null ? default : JsonSerializer.Deserialize<T>(cached);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expire = null, CancellationToken ct = default)
        {
            var data = JsonSerializer.Serialize(value);

            var options = new DistributedCacheEntryOptions();
            if (expire != null)
                options.SetAbsoluteExpiration(expire.Value);

            await _cache.SetStringAsync(key, data, options, ct);
        }

        public Task RemoveAsync(string key, CancellationToken ct = default)
            => _cache.RemoveAsync(key, ct);
    }
}