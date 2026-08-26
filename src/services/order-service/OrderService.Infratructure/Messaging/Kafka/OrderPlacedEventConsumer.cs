using CatalogShared.Models;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Options;
using OrderService.Application.Abstractions.Services;
using OrderService.Application.Features.OrderFeatures.Commands.CreateOrder;
using System.Text.Json;

namespace OrderService.Infratructure.Messaging.Kafka
{
    public class OrderPlacedEventConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderPlacedEventConsumer> _logger;
        private readonly KafkaConsumerOptions _kafkaOptions;
        private const string TOPIC_NAME = "order.placed";

        public OrderPlacedEventConsumer(
            IServiceProvider serviceProvider,
            ILogger<OrderPlacedEventConsumer> logger,
            IOptions<KafkaConsumerOptions> kafkaOptions)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _kafkaOptions = kafkaOptions.Value;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!Enum.TryParse(_kafkaOptions.AutoOffsetReset, true, out AutoOffsetReset autoOffsetResetEnum))
            {
                // Nếu chuỗi cấu hình sai, chọn một giá trị mặc định an toàn
                autoOffsetResetEnum = AutoOffsetReset.Earliest;
            }
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaOptions.BootstrapServers,
                GroupId = _kafkaOptions.GroupId,
                AutoOffsetReset = autoOffsetResetEnum,
                EnableAutoCommit = _kafkaOptions.EnableAutoCommit,
                MaxPollIntervalMs = _kafkaOptions.MaxPollIntervalMs,
                SessionTimeoutMs = _kafkaOptions.SessionTimeoutMs,
            };

            var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(TOPIC_NAME);

            _logger.LogInformation("OrderPlacedEventConsumer started on {Topic}", TOPIC_NAME);

            return Task.Run(async () =>
            {
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var result = consumer.Consume(stoppingToken);
                            if (result?.Message == null) continue;

                            await ProcessMessageAsync(result.Message, stoppingToken);
                            consumer.Commit(result);
                        }
                        catch (ConsumeException ex)
                        {
                            _logger.LogError(ex, "Kafka consume error");
                            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                        }
                    }
                }
                finally
                {
                    consumer.Close();
                }
            }, stoppingToken);
        }

        private async Task ProcessMessageAsync(Message<string, string> kafkaMessage, CancellationToken ct)
        {
            using var scope = _serviceProvider.CreateScope();
            var inboxRepository = scope.ServiceProvider.GetRequiredService<IInboxRepository>(); // Repository
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var orderId = Guid.Parse(kafkaMessage.Key);

            // Idempotency check qua repository
            if (await inboxRepository.IsProcessedAsync(orderId, ct))
            {
                _logger.LogWarning("Duplicate OrderPlacedEvent {OrderId}, skipping", orderId);
                return;
            }

            try
            {
                var orderEvent = JsonSerializer.Deserialize<OrderPlaceEvent>(
                    kafkaMessage.Value,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

                var command = new CreateOrderFromKafkaCommand(orderEvent);
                await mediator.Send(command, ct);

                // Đánh dấu processed SAU KHI handler thành công
                await inboxRepository.MarkAsProcessedAsync(orderId, ct);
                _logger.LogInformation("Processed OrderPlacedEvent {OrderId}", orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process OrderPlacedEvent {OrderId}", orderId);

                // Best-effort mark failed qua repository
                await inboxRepository.MarkAsFailedAsync(orderId, ex.Message, ct);
            }
        }
    }
}
