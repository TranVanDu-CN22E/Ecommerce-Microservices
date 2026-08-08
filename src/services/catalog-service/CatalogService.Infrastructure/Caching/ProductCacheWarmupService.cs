using CatalogService.Application.Abstractions.Services;

namespace CatalogService.Infrastructure.Caching
{
    public class ProductCacheWarmupService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<ProductCacheWarmupService> _logger;

        public ProductCacheWarmupService(IServiceProvider sp, ILogger<ProductCacheWarmupService> logger)
        {
            _sp = sp;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Chiến lược warmup: Chỉ load các sản phẩm published, có traffic cao
            // Hoặc dùng event-driven: khi product updated → invalidate cache
            // Dưới đây là ví dụ periodic consistency check

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _sp.CreateScope();
                    var cache = scope.ServiceProvider.GetRequiredService<IProductCacheService>();
                    var readRepo = scope.ServiceProvider.GetRequiredService<IProductReadRepository>();

                    // Ví dụ: Re-sync các product hot mỗi 10 phút
                    // Trong production, nên dùng Redis Keyspace Notifications hoặc CDC (Debezium)
                    // thay vì polling

                    _logger.LogDebug("Product cache consistency check completed");
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Error in ProductCacheWarmupService");
                }

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}
