using Microsoft.Extensions.Options;
using OrderService.Application.Abstractions.Services;
using OrderService.Application.DTOs;
using OrderService.Domain.Aggregates.OrderAggregate;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderService.Infratructure.Caching
{
    public sealed class RedisShippingRuleCacheService : IShippingRuleCacheService
    {
        private readonly StackExchange.Redis.IDatabase _database;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly RedisOptions _options;
        private const string SHIPPING_RULE_KEY_PATTERN = "shippingrule:{0}";
        // Tối ưu: Dùng chung cấu hình JSON để tránh cấp phát bộ nhớ liên tục
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public RedisShippingRuleCacheService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisOptions> options)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _options = options.Value;
            _database = _connectionMultiplexer.GetDatabase();

        }
        private string GetKey(string ruleId) => $"{_options.KeyPrefix}{string.Format(SHIPPING_RULE_KEY_PATTERN, ruleId)}";
        private string GetScanPattern() => $"{_options.KeyPrefix}{string.Format(SHIPPING_RULE_KEY_PATTERN, "*")}";
        public async Task<bool> AddListShippingRulesAsync(List<ShippingRuleDto> rules, CancellationToken cancellationToken = default)
        {
            if (rules == null || !rules.Any()) return false;

            // Serialize and create tasks immediately
            var tasks = rules.Select(rule =>
            {
                var key = GetKey(rule.Id);
                var serializedShippingRule = JsonSerializer.Serialize(rule, _jsonOptions);
                return _database.StringSetAsync(key, serializedShippingRule, TimeSpan.FromMinutes(_options.DefaultTTLMinutes));
            }).ToList();

            // Await all tasks concurrently without blocking
            await Task.WhenAll(tasks);
            return true;
        }


        public async Task<bool> AddShippingRuleAsync(string ruleId, ShippingRuleDto rule, CancellationToken cancellationToken = default)
        {
            var key = GetKey(ruleId);
            var serializedShippingRule = JsonSerializer.Serialize(rule, _jsonOptions);
            return await _database.StringSetAsync(key, serializedShippingRule, TimeSpan.FromMinutes(_options.DefaultTTLMinutes));
        }

        public async Task<List<ShippingRuleDto>> GetListShippingRulesAsync(CancellationToken cancellationToken = default)
        {
            var results = new List<ShippingRuleDto>();
            var endpoints = _connectionMultiplexer.GetEndPoints();
            var pattern = GetScanPattern();
            var keys = new List<RedisKey>();

            // Quét tìm tất cả các Key khớp với Pattern từ các Node Redis (Hỗ trợ tốt cho cả Redis Cluster)
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                // Dùng các tham số mặc định của SE.Redis (pageSize: 250) để tối ưu hiệu năng
                var serverKeys = server.Keys(_database.Database, pattern);
                keys.AddRange(serverKeys);
            }

            // Loại bỏ các key trùng lặp nếu có giữa các node
            var distinctKeys = keys.Distinct().ToArray();
            if (!distinctKeys.Any()) return results;

            // Lấy dữ liệu hàng loạt (MGET) để tối ưu hóa số lượng Request gửi lên Redis
            var values = await _database.StringGetAsync(distinctKeys);

            // Deserialize dữ liệu trả về danh sách DTO
            foreach (var value in values)
            {
                if (!value.IsNullOrEmpty)
                {
                    var dto = JsonSerializer.Deserialize<ShippingRuleDto>(value.ToString(), _jsonOptions);
                    if (dto != null)
                    {
                        results.Add(dto);
                    }
                }
            }
            return results;
        }

        public async Task<ShippingRuleDto?> GetShippingRuleAsync(string ruleId, CancellationToken cancellationToken = default)
        {
            var key = GetKey(ruleId);

            var shippingRuleJson = await _database.StringGetAsync(key);
            if (shippingRuleJson.IsNullOrEmpty)
            {
                return null;
            }
            return JsonSerializer.Deserialize<ShippingRuleDto>(shippingRuleJson.ToString());
        }

        public async Task<bool> RemoveShippingRuleAsync(string ruleId, CancellationToken cancellationToken = default)
        {
            var key = GetKey(ruleId);
            return await _database.KeyDeleteAsync(key);
        }
    }
}
