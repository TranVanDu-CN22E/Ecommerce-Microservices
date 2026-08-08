using CatalogService.Application.Abstractions.Services;
using CatalogService.Application.Interfaces.Storage;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Caching;
using CatalogService.Infrastructure.Persistence;
using CatalogService.Infrastructure.Persistence.Repositories;
using CatalogService.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

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

            var redisConfig = configuration.GetSection("Redis").Get<RedisOptions>();

            // Singleton ConnectionMultiplexer để tái sử dụng connection
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(redisConfig.ConnectionString));

            services.Configure<RedisOptions>(configuration.GetSection("Redis"));
            services.AddScoped<IProductCacheService, RedisProductCacheService>();

            // Đăng ký BackgroundService
            services.AddHostedService<ProductCacheWarmupService>();
            return services;
        }
    }
}
