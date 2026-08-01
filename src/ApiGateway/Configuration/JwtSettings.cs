namespace ApiGateway.Configuration
{
    public sealed class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        public string Key { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public bool ValidateIssuer { get; init; } = true;
        public bool ValidateAudience { get; init; } = true;
        public bool ValidateLifetime { get; init; } = true;
        public bool ValidateIssuerSigningKey { get; init; } = true;
    }
}
