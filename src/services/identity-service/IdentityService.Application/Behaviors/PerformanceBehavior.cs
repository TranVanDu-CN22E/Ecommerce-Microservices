using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace IdentityService.Application.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    private const int WarningThresholdMs = 500;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next();
            return response;
        }
        finally
        {
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > WarningThresholdMs)
            {
                _logger.LogWarning(
                    "Long running request {RequestName} took {ElapsedMilliseconds} ms",
                    typeof(TRequest).Name,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
