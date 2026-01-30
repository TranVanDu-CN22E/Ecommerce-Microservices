namespace ApiGateway.Configuration
{
    public sealed class GatewaySettings
    {
        public const string SectionName = "Gateway";

        public string Name { get; init; } = "API Gateway";
        public string Version { get; init; } = "1.0.0";
        public bool EnableSwagger { get; init; } = true;
        public bool EnableHealthChecks { get; init; } = true;
        public CorsSettings Cors { get; init; } = new();
        public RateLimitSettings RateLimit { get; init; } = new();
    }
}
