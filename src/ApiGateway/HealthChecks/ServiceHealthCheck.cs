using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiGateway.HealthChecks
{
    public sealed class ServiceHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _serviceName;
        private readonly string _healthEndpoint;

        public ServiceHealthCheck(
            IHttpClientFactory httpClientFactory,
            string serviceName,
            string healthEndpoint)
        {
            _httpClientFactory = httpClientFactory;
            _serviceName = serviceName;
            _healthEndpoint = healthEndpoint;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(5);

                var response = await client.GetAsync(_healthEndpoint, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy(
                        $"{_serviceName} is healthy");
                }

                return HealthCheckResult.Unhealthy(
                    $"{_serviceName} returned status code {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    $"{_serviceName} is unreachable: {ex.Message}");
            }
        }
    }
}
