namespace OrderService.Infratructure.Caching
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }
        public int DefaultTTLMinutes { get; set; } = 5;
        public string KeyPrefix { get; set; } = "Order:";
    }
}
