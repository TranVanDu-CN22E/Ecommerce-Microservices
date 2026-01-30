using System.Diagnostics;

namespace ApiGateway.Middleware
{
    public sealed class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString();

            context.Items["RequestId"] = requestId;
            context.Response.Headers["X-Request-ID"] = requestId;

            try
            {
                _logger.LogInformation(
                    "Incoming request: {Method} {Path} - RequestId: {RequestId}",
                    context.Request.Method,
                    context.Request.Path,
                    requestId);

                await _next(context);

                stopwatch.Stop();

                _logger.LogInformation(
                    "Completed request: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms - RequestId: {RequestId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds,
                    requestId);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                    ex,
                    "Request failed: {Method} {Path} - Duration: {Duration}ms - RequestId: {RequestId}",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds,
                    requestId);

                throw;
            }
        }
    }
}
