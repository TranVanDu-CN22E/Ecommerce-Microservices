namespace CatalogService.Infrastructure.Caching
{
    public sealed class RedisOptions
    {
        public string ConnectionString { get; set; }
        public int DefaultTTLMinutes { get; set; } = 5;
        public string KeyPrefix { get; set; } = "catalog:";
    }
}
