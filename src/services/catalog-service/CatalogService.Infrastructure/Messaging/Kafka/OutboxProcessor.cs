using CatalogService.Infrastructure.Persistence.Outbox;
using Confluent.Kafka;
using global::CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CatalogService.Infrastructure.Messaging.Kafka
{

    internal sealed class OutboxProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutboxProcessor> _logger;
        private readonly IProducer<string, string> _kafkaProducer;
        private const string TOPIC_NAME = "order.placed";

        public OutboxProcessor(IServiceProvider serviceProvider, ILogger<OutboxProcessor> logger, IProducer<string, string> kafkaProducer)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _kafkaProducer = kafkaProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

                    // Lấy batch messages chưa xử lý
                    var messages = await dbContext.Set<OutboxMessage>()
                        .Where(m => m.ProcessedOnUtc == null)
                        .OrderBy(m => m.OccurredOnUtc)
                        .Take(100)
                        .ToListAsync(stoppingToken);

                    foreach (var message in messages)
                    {
                        try
                        {
                            await _kafkaProducer.ProduceAsync(TOPIC_NAME, new Message<string, string>
                            {
                                Key = message.Id.ToString(), // Dùng OutboxId làm key để đảm bảo partition ordering nếu cần
                                Value = message.Content
                            }, stoppingToken);

                            message.ProcessedOnUtc = DateTime.UtcNow;
                            _logger.LogInformation("Published OutboxMessage {Id} to Kafka", message.Id);
                        }
                        catch (Exception ex)
                        {
                            message.Error = ex.Message;
                            _logger.LogError(ex, "Failed to publish OutboxMessage {Id}", message.Id);
                            // Có thể implement retry count / dead-letter queue tại đây
                        }
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing outbox messages");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken); // Polling interval
            }
        }
    }
}