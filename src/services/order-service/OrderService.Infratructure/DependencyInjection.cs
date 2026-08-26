using OrderService.Infratructure.Persistence;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Interface;
using OrderService.Infratructure.Persistence.Repository;
using OrderService.Application.Abstractions.Services;
using OrderService.Infratructure.Messaging.Kafka;
namespace OrderService.Infratructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IShippingRuleRepository, ShippingRuleRepository>();
            services.AddScoped<IInboxRepository, InboxRepository>();
            services.Configure<KafkaConsumerOptions>(configuration.GetSection("KafkaConsumer"));
            services.AddHostedService<OrderPlacedEventConsumer>();


            return services;
        }
    }
}
