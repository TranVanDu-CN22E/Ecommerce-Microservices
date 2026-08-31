using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using OrderService.Application.Abstractions.Services;
using OrderService.Application.DTOs;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderService.Infratructure.Caching
{
    public sealed class RedisOrderCacheService : IOrderCacheService
    {
        private readonly StackExchange.Redis.IDatabase _database;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly RedisOptions _options;

        private const string ORDER_KEY_PATTERN = "order:{0}";
        private const string USER_ORDERS_KEY_PATTERN = "user:{0}:orders";

        // Cấu hình chung cho Serialization để đồng bộ CamelCase với UI/API
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public RedisOrderCacheService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisOptions> options)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _options = options.Value;
            _database = _connectionMultiplexer.GetDatabase();
        }

        private string GetKey(string orderId) => $"{_options.KeyPrefix}{string.Format(ORDER_KEY_PATTERN, orderId)}";
        private string GetUserOrdersKey(string userId) => $"{_options.KeyPrefix}{string.Format(USER_ORDERS_KEY_PATTERN, userId)}";

        // Lấy chi tiết 1 đơn hàng lẻ
        public async Task<GetOrderDto?> GetOrderAsync(string orderId, CancellationToken cancellationToken)
        {
            var key = GetKey(orderId);
            var orderJson = await _database.StringGetAsync(key);

            if (orderJson.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<GetOrderDto>(orderJson.ToString(), _jsonOptions);
        }

        // Xóa đơn hàng lẻ (Đồng thời đập luôn cache danh sách tổng của user đó)
        public async Task<bool> RemoveOrderAsync(string orderId, CancellationToken cancellationToken)
        {
            // Tìm thông tin đơn hàng trước để lấy CustomerId
            var orderDto = await GetOrderAsync(orderId, cancellationToken);
            if (orderDto != null && !string.IsNullOrEmpty(orderDto.CustomerId))
            {
                var userOrdersKey = GetUserOrdersKey(orderDto.CustomerId);
                // XÓA SẠCH: Đập bỏ cái nhà chung chứa chuỗi JSON list đơn của user này
                await _database.KeyDeleteAsync(userOrdersKey);
            }

            var orderKey = GetKey(orderId);
            return await _database.KeyDeleteAsync(orderKey); // Xóa tiếp chi tiết đơn lẻ
        }

        // Lưu đơn hàng mới lẻ (Đổi lại thành public để đáp ứng Interface)
        public async Task<bool> SetOrderAsync(string orderId, GetOrderDto orderDto, CancellationToken cancellationToken)
        {
            var key = GetKey(orderId); // Đã sửa: Có Prefix đồng bộ
            var ttl = TimeSpan.FromMinutes(_options.DefaultTTLMinutes);
            var serializedOrder = JsonSerializer.Serialize(orderDto, _jsonOptions);

            var isSetSuccess = await _database.StringSetAsync(key, serializedOrder, ttl);

            // Nếu lưu đơn lẻ thành công, xóa ngay cache danh sách tổng của User đó 
            // để lần sau họ vào trang danh sách, hệ thống bắt buộc phải xuống SQL DB cập nhật lại list mới nhất
            if (isSetSuccess && !string.IsNullOrEmpty(orderDto.CustomerId))
            {
                var userOrdersKey = GetUserOrdersKey(orderDto.CustomerId);
                await _database.KeyDeleteAsync(userOrdersKey);
            }

            return isSetSuccess;
        }

        // Lấy nguyên cả chuỗi JSON list đơn hàng của User
        public async Task<List<GetOrderDto>?> GetOrdersByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var userOrdersKey = GetUserOrdersKey(userId);
            var listJson = await _database.StringGetAsync(userOrdersKey);

            if (listJson.IsNullOrEmpty)
            {
                return null;
            }
            return JsonSerializer.Deserialize<List<GetOrderDto>>(listJson.ToString(), _jsonOptions);
        }

        // Nạp nguyên cả cụm list dữ liệu lấy từ SQL DB lên vào 1 Key duy nhất
        public async Task<bool> SetListOrderAsync(string userId, List<GetOrderDto> orderDtos, CancellationToken cancellationToken)
        {
            var userOrdersKey = GetUserOrdersKey(userId);
            var ttl = TimeSpan.FromMinutes(_options.DefaultTTLMinutes);

            // Ép nguyên cả List thành 1 chuỗi JSON dài
            var serializedList = JsonSerializer.Serialize(orderDtos, _jsonOptions);

            // Lưu vào Redis dưới dạng String thông thường với 1 key duy nhất
            return await _database.StringSetAsync(userOrdersKey, serializedList, ttl);
        }

        // Hàm giả định cho trường hợp Interface cũ của bạn bắt truyền vào List (Không có UserId)
        public async Task<bool> SetListOrderAsync(List<GetOrderDto> orderDtos, CancellationToken cancellationToken)
        {
            if (orderDtos == null || !orderDtos.Any()) return true;

            // Gom nhóm các đơn theo CustomerId rồi nạp vào từng Key tương ứng
            var groupedOrders = orderDtos.GroupBy(o => o.CustomerId);
            foreach (var group in groupedOrders)
            {
                if (!string.IsNullOrEmpty(group.Key))
                {
                    await SetListOrderAsync(group.Key, group.ToList(), cancellationToken);
                }
            }
            return true;
        }

        public async Task<bool> ExistsOrderAsync(string orderId, CancellationToken cancellationToken)
        {
            var key = GetKey(orderId);
            return await _database.KeyExistsAsync(key);
        }
    }
}
