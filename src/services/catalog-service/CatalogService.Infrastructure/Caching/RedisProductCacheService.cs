using CatalogService.Application.Abstractions.Services;
using CatalogService.Application.DTOs;
using StackExchange.Redis;
using System.Text.Json;

namespace CatalogService.Infrastructure.Caching
{
    public sealed class RedisProductCacheService : IProductCacheService
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly RedisOptions _options;
        private const string PRODUCT_KEY_PATTERN = "product:{0}";
        public RedisProductCacheService(IConnectionMultiplexer connectionMultiplexer, RedisOptions options)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _options = options;
            _database = _connectionMultiplexer.GetDatabase();
        }
        private string GetKey(string productId) => $"{_options.KeyPrefix}{string.Format(PRODUCT_KEY_PATTERN, productId)}";
        // Lua Script: Atomic Check & Reserve
        // KEYS[1] = product key
        // ARGV[1] = variantId, ARGV[2] = quantity, ARGV[3] = ttlSeconds
        private const string RESERVE_SCRIPT = @"
            -- Kiểm tra key tồn tại
            local key = KEYS[1]
            if redis.call('EXISTS', key) == 0 then return -1 end

            -- Kiểm tra quantity hợp lệ
            local qty = tonumber(ARGV[2])
            if qty == nil or qty <= 0 then
                return -3
            end

            -- Lấy dữ liệu sản phẩm và biến thể
            local json = redis.call('GET', key)
            local product = cjson.decode(json)
            local variant = nil
            for _, v in ipairs(product.variants) do
                if v.productVariantId == ARGV[1] then
                    variant = v
                    break
                end
            end
            
            -- Kiểm tra biến thể tồn tại
            if variant == nil then return -2 end

            -- Kiểm tra số lượng khả dụng
            local available = variant.stockQuantity - variant.reservedQuantity
        
            -- Nếu số lượng khả dụng nhỏ hơn số lượng yêu cầu, trả về 0
            if available < qty then return 0 end
        
            -- Cập nhật số lượng đã đặt trước và lưu lại với TTL
            variant.reservedQuantity = variant.reservedQuantity + qty
            redis.call('SET', key, cjson.encode(product), 'KEEPTTL')
            return 1
            end";
        public async Task<bool> SetProductAsync(string productId, ProductResponseDto product, CancellationToken cancellationToken = default)
        {
            var key = GetKey(productId);
            var serializedProduct = JsonSerializer.Serialize(product, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return await _database.StringSetAsync(key, serializedProduct, TimeSpan.FromMinutes(_options.DefaultTTLMinutes));
        }
        public Task<bool> DecrementReservedCountAsync(string productId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductAsync(string productId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductResponseDto?> GetProductAsync(string productId, CancellationToken cancellationToken = default)
        {
            var key = GetKey(productId);
            var value = await _database.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return null;
            await _database.KeyExpireAsync(key, TimeSpan.FromMinutes(_options.DefaultTTLMinutes));
            return JsonSerializer.Deserialize<ProductResponseDto>(value!, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public Task<int> GetReservedCountAsync(string productId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IncrementReservedCountAsync(string productId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsProductExistsAsync(string productId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RefreshTtlAsync(string productId, CancellationToken cancellationToken = default)
        {
            var key = GetKey(productId);
            return await _database.KeyExpireAsync(key, TimeSpan.FromMinutes(_options.DefaultTTLMinutes));
        }
        public Task<bool> UpdateVariantReservedAsync(string productId, string variantSku, int quantityChange, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateVariantStockAsync(string productId, string variantSku, int quantityChange, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
