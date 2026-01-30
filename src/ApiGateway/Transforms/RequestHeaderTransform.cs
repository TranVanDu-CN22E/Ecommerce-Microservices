using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace ApiGateway.Transforms
{
    public sealed class RequestHeaderTransform : ITransformProvider
    {
        public void ValidateRoute(TransformRouteValidationContext context)
        {
            // Validation logic if needed
        }

        public void ValidateCluster(TransformClusterValidationContext context)
        {
            // Validation logic if needed
        }

        public void Apply(TransformBuilderContext context)
        {
            // Add correlation ID to all requests
            context.AddRequestTransform(async transformContext =>
            {
                var correlationId = transformContext.HttpContext.Items["RequestId"]?.ToString()
                    ?? Guid.NewGuid().ToString();

                transformContext.ProxyRequest.Headers.Add("X-Correlation-ID", correlationId);
                transformContext.ProxyRequest.Headers.Add("X-Gateway", "YARP-Gateway");

                await Task.CompletedTask;
            });

            // Add response headers
            context.AddResponseTransform(async transformContext =>
            {
                transformContext.HttpContext.Response.Headers.Add(
                    "X-Gateway-Version", "1.0.0");

                await Task.CompletedTask;
            });
        }
    }
}
