using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;
using Microsoft.AspNetCore.Http; // Thêm thư viện này nếu cần cho .Append()

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

                // SỬA TẠI ĐÂY: Dùng TryAddWithoutValidation để không bị lỗi nếu key đã tồn tại
                transformContext.ProxyRequest.Headers.TryAddWithoutValidation("X-Correlation-ID", correlationId);
                transformContext.ProxyRequest.Headers.TryAddWithoutValidation("X-Gateway", "YARP-Gateway");

                await Task.CompletedTask;
            });

            // Add response headers
            context.AddResponseTransform(async transformContext =>
            {
                // SỬA TẠI ĐÂY: Sử dụng Indexer hoặc .Append() cho Response Headers theo chuẩn ASP.NET Core
                transformContext.HttpContext.Response.Headers["X-Gateway-Version"] = "1.0.0";

                await Task.CompletedTask;
            });
        }
    }
}
