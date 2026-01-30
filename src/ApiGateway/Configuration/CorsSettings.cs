namespace ApiGateway.Configuration
{
    public sealed class CorsSettings
    {
        public string[] AllowedOrigins { get; init; } = Array.Empty<string>();
        public string[] AllowedMethods { get; init; } = Array.Empty<string>();
        public string[] AllowedHeaders { get; init; } = Array.Empty<string>();
        public bool AllowCredentials { get; init; }
    }
}
