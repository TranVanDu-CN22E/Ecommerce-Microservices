using System.Net;
using System.Text.Json;

namespace ApiGateway.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(
                exception,
                "An unhandled exception occurred - RequestId: {RequestId}",
                context.Items["RequestId"]);

            var response = new ErrorResponse
            {
                Message = "An error occurred while processing your request.",
                RequestId = context.Items["RequestId"]?.ToString() ?? string.Empty,
                Timestamp = DateTime.UtcNow
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

        private sealed class ErrorResponse
        {
            public string Message { get; init; } = string.Empty;
            public string RequestId { get; init; } = string.Empty;
            public DateTime Timestamp { get; init; }
        }
    }
}
