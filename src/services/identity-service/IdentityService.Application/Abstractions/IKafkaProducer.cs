namespace IdentityService.Application.Abstractions
{
    public interface IKafkaProducer
    {
        Task PublishAsync<T>(string topic, T message, CancellationToken ct = default);

    }
}
