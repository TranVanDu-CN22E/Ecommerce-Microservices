namespace ApiGateway.Configuration
{
    public sealed class RateLimitSettings
    {
        public bool Enabled { get; init; }
        public int PermitLimit { get; init; } = 100;
        public int WindowSeconds { get; init; } = 60;
        public int QueueLimit { get; init; } = 10;
    }
}
