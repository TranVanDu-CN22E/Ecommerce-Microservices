using CatalogService.Application.Abstractions.Services;
using CatalogService.Application.Interfaces.Storage;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Caching;
using CatalogService.Infrastructure.GRPC;
using CatalogService.Infrastructure.Messaging.Kafka;
using CatalogService.Infrastructure.Persistence;
using CatalogService.Infrastructure.Persistence.Repositories;
using CatalogService.Infrastructure.Storage;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CatalogService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("Default")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IVariantAttributeRepository, VariantAttributeRepository>();
            // Đảm bảo tên chuỗi "StorageSettings" trùng khớp chính xác với Key trong file appsettings.json
            services.Configure<LocalFileStorageOptions>(configuration.GetSection("LocalFileStorage"));
            services.AddTransient<ILocalFileStorage, LocalFileStorage>();
            //GRPC Client for IdentityService
            services.AddGrpcClient<IdentityShared.Protos.UserGrpcService.UserGrpcServiceClient>(options =>
            {
                // Địa chỉ của IdentityService
                options.Address = new Uri(configuration["GrpcSettings:IdentityServiceUrl"] ?? "http://identity-api:8081");
            });
            services.AddScoped<CatalogService.Application.Interfaces.GRPC.IIdentityService, CatalogService.Infrastructure.GRPC.IdentityService>();

            //services.Configure<RedisOptions>(configuration.GetSection("Redis"));
            var redisConfig = configuration.GetSection("Redis").Get<RedisOptions>();
            // Singleton ConnectionMultiplexer để tái sử dụng connection
            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConfig.ConnectionString));

            services.AddScoped<IProductCacheService, RedisProductCacheService>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();

            // Đọc toàn bộ cấu hình từ appsettings.json một cách an toàn
            var kafkaOptions = configuration.GetSection("Kafka").Get<KafkaOptions>()
                ?? throw new InvalidOperationException("Kafka configuration is missing.");
            // Map các thông số vào ProducerConfig của Confluent.Kafka
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaOptions.BootstrapServers,
                ClientId = kafkaOptions.ClientId,
                Acks = kafkaOptions.Acks.ToLower() == "all" ? Confluent.Kafka.Acks.All : Confluent.Kafka.Acks.None,
                EnableIdempotence = kafkaOptions.EnableIdempotence,
                MessageSendMaxRetries = kafkaOptions.MessageSendMaxRetries,
                RetryBackoffMs = kafkaOptions.RetryBackoffMs,
                LingerMs = kafkaOptions.LingerMs,
                BatchNumMessages = kafkaOptions.BatchNumMessages
            };
            // Đăng ký Singleton chuẩn xác (DI tự động quản lý vòng đời và tự Dispose khi app tắt)
            services.AddSingleton<IProducer<string, string>>(sp =>
                new ProducerBuilder<string, string>(config).Build());


            // Đăng ký BackgroundService
            services.AddHostedService<ProductCacheWarmupService>();

            services.AddHostedService<OutboxProcessor>();
            return services;
        }
    }
}
