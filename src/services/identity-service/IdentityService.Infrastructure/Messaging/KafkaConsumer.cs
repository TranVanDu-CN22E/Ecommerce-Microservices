using Confluent.Kafka;
using IdentityService.Infrastructure.Inbox;

namespace IdentityService.Infrastructure.Messaging
{

    public class KafkaConsumer : BackgroundService
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly KafkaSettings _settings;
        private readonly InboxProcessor _inbox;

        public KafkaConsumer(
            ILogger<KafkaConsumer> logger,
            KafkaSettings settings,
            InboxProcessor inbox)
        {
            _logger = logger;
            _settings = settings;
            _inbox = inbox;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                GroupId = _settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(_settings.Topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(stoppingToken);
                    _logger.LogInformation("Kafka Received: {Value}", cr.Message.Value);

                    await _inbox.ProcessAsync(cr.Message.Key!, cr.Message.Value, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Kafka consumer error.");
                }
            }
        }
    }
}
