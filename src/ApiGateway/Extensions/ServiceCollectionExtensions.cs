using ApiGateway.Configuration;
using ApiGateway.HealthChecks;
using ApiGateway.Transforms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

namespace ApiGateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGatewayServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configuration
            var gatewaySettings = configuration
                .GetSection(GatewaySettings.SectionName)
                .Get<GatewaySettings>() ?? new GatewaySettings();

            services.Configure<GatewaySettings>(
                configuration.GetSection(GatewaySettings.SectionName));

            // YARP Reverse Proxy
            services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"))
                .AddTransforms<RequestHeaderTransform>();

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("GatewayPolicy", builder =>
                {
                    var corsSettings = gatewaySettings.Cors;

                    if (corsSettings.AllowedOrigins.Any())
                    {
                        builder.WithOrigins(corsSettings.AllowedOrigins);
                    }
                    else
                    {
                        builder.AllowAnyOrigin();
                    }

                    if (corsSettings.AllowedMethods.Any())
                    {
                        builder.WithMethods(corsSettings.AllowedMethods);
                    }
                    else
                    {
                        builder.AllowAnyMethod();
                    }

                    if (corsSettings.AllowedHeaders.Any())
                    {
                        builder.WithHeaders(corsSettings.AllowedHeaders);
                    }
                    else
                    {
                        builder.AllowAnyHeader();
                    }

                    if (corsSettings.AllowCredentials)
                    {
                        builder.AllowCredentials();
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddGatewayAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration
                .GetSection(JwtSettings.SectionName)
                .Get<JwtSettings>();

            if (jwtSettings is null)
                throw new InvalidOperationException("JWT settings are not configured");

            services.Configure<JwtSettings>(
                configuration.GetSection(JwtSettings.SectionName));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = jwtSettings.ValidateIssuer,
                        ValidateAudience = jwtSettings.ValidateAudience,
                        ValidateLifetime = jwtSettings.ValidateLifetime,
                        ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddGatewayRateLimiting(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var gatewaySettings = configuration
                .GetSection(GatewaySettings.SectionName)
                .Get<GatewaySettings>() ?? new GatewaySettings();

            if (!gatewaySettings.RateLimit.Enabled)
                return services;

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddFixedWindowLimiter("fixed", limiterOptions =>
                {
                    limiterOptions.PermitLimit = gatewaySettings.RateLimit.PermitLimit;
                    limiterOptions.Window = TimeSpan.FromSeconds(gatewaySettings.RateLimit.WindowSeconds);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = gatewaySettings.RateLimit.QueueLimit;
                });

                options.AddPolicy("PerUserRateLimit", context =>
                {
                    var userId = context.User?.FindFirst("sub")?.Value ?? "anonymous";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        userId,
                        _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = gatewaySettings.RateLimit.PermitLimit,
                            Window = TimeSpan.FromSeconds(gatewaySettings.RateLimit.WindowSeconds),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = gatewaySettings.RateLimit.QueueLimit
                        });
                });
            });

            return services;
        }

        public static IServiceCollection AddGatewayHealthChecks(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var healthChecksBuilder = services.AddHealthChecks();

            // Add service health checks
            var routes = configuration.GetSection("ReverseProxy:Routes").Get<Dictionary<string, RouteConfig>>();

            if (routes != null)
            {
                foreach (var route in routes)
                {
                    var healthEndpoint = route.Value.Metadata?.GetValueOrDefault("HealthEndpoint");
                    if (!string.IsNullOrEmpty(healthEndpoint))
                    {
                        healthChecksBuilder.Add(new Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckRegistration(
                            route.Key,
                            sp => new ServiceHealthCheck(
                                sp.GetRequiredService<IHttpClientFactory>(),
                                route.Key,
                                healthEndpoint),
                            null,
                            new[] { "services" }));
                    }
                }
            }

            return services;
        }

        private class RouteConfig
        {
            public Dictionary<string, string>? Metadata { get; set; }
        }
    }
}
