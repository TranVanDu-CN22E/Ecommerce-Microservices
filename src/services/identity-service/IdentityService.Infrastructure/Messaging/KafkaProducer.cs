using Confluent.Kafka;
using IdentityService.Application.Abstractions;
using System.Text.Json;

namespace IdentityService.Infrastructure.Messaging
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;
        private readonly KafkaSettings _settings;

        public KafkaProducer(ILogger<KafkaProducer> logger, KafkaSettings settings)
        {
            _logger = logger;
            _settings = settings;

            var config = new ProducerConfig
            {
                BootstrapServers = settings?.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync<T>(string topic, T message, CancellationToken ct = default)
        {
            var json = JsonSerializer.Serialize(message);

            var kafkaMessage = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = json
            };

            await _producer.ProduceAsync(topic, kafkaMessage, ct);

            _logger.LogInformation("Published Kafka message to {Topic}", topic);
        }
    }
}
