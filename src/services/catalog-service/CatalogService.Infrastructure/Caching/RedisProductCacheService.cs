using CatalogService.Application.Abstractions.Services;
using CatalogService.Application.DTOs;
using CatalogService.Domain.Aggregates.ProductAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
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
        public RedisProductCacheService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisOptions> options)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _options = options.Value;
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

        /// <summary>
        /// </summary>
        /// <param name="productId">KEYS[1]: Khóa(Key) của sản phẩm, được tạo ra từ hàm GetKey(productId).</param>
        /// <param name="variantId">ARGV[1]: Mã phiên bản sản phẩm(variantId).</param>
        /// <param name="quantity">ARGV[2]: Số lượng cần giải phóng(quantity).</param>
        /// <returns></returns>
        public async Task ReleaseReservedAsync(string productId, string variantId, int quantity)
        {
            // Nếu reservedQuantity về 0 và không còn ai giữ → key tự expire theo TTL
            var script = @"
                local key = KEYS[1]
                if redis.call('EXISTS', key) == 0 then return 0 end
                local json = redis.call('GET', key)
                local product = cjson.decode(json)
                for _, v in ipairs(product.variants) do
                    if v.productVariantId == ARGV[1] then
                        v.reservedQuantity = math.max(0, v.reservedQuantity - tonumber(ARGV[2]))
                    end
                end
                redis.call('SET', key, cjson.encode(product), 'KEEPTTL')
                return 1
                ";
            await _database.ScriptEvaluateAsync(script, new RedisKey[] { GetKey(productId) }, new RedisValue[] { variantId, quantity });
        }
        /// <summary>
        /// Đặt gạch/giữ chỗ trước khi mua hàng.
        /// Đảm bảo tính Atomic (nguyên tử), giúp hệ thống chống Overbooking (bán quá số lượng tồn kho).
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="variantId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task<bool> TryReserveStockAsync(string productId, string variantId, int quantity)
        {
            var key = GetKey(productId);
            var result = await _database.ScriptEvaluateAsync(RESERVE_SCRIPT,
                new RedisKey[] { key },
                new RedisValue[] { variantId, quantity, _options.DefaultTTLMinutes * 60 });

            return (long)result == 1;
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
        public async Task<bool> RefreshTtlAsync(string productId, CancellationToken cancellationToken = default)
        {
            var key = GetKey(productId);
            return await _database.KeyExpireAsync(key, TimeSpan.FromMinutes(_options.DefaultTTLMinutes));
        }

        public async Task ConfirmPurchaseAsync(string productId, string variantId, int quantity)
        {
            // Giảm cả stockQuantity lẫn reservedQuantity
            var script = @"
            local key = KEYS[1]
            if redis.call('EXISTS', key) == 0 then return 0 end
            local json = redis.call('GET', key)
            local product = cjson.decode(json)
            for _, v in ipairs(product.variants) do
                if v.productVariantId == ARGV[1] then
                    v.stockQuantity = v.stockQuantity - tonumber(ARGV[2])
                    v.reservedQuantity = math.max(0, v.reservedQuantity - tonumber(ARGV[2]))
                    break
                end
            end
            redis.call('SET', key, cjson.encode(product), 'KEEPTTL')
            return 1
        ";
            await _database.ScriptEvaluateAsync(script, new RedisKey[] { GetKey(productId) }, new RedisValue[] { variantId, quantity });
        }
    }
}
