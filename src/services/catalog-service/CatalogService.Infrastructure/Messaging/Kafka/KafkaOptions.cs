namespace CatalogService.Infrastructure.Messaging.Kafka
{
    public class KafkaOptions
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string Acks { get; set; } = "All";
        public bool EnableIdempotence { get; set; } = true;
        public int MessageSendMaxRetries { get; set; } = 3;
        public int RetryBackoffMs { get; set; } = 100;
        public double LingerMs { get; set; } = 10;
        public int BatchNumMessages { get; set; } = 1000;
    }
}
