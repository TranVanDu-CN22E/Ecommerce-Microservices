using ApiGateway.Extensions;
using ApiGateway.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "ApiGateway")
    .WriteTo.Console()
    .WriteTo.File(
        "logs/gateway-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .CreateLogger();

builder.Host.UseSerilog();
try { 
    Log.Information("Starting ApiGateway host");

    builder.Services.AddGatewayServices(builder.Configuration);
    builder.Services.AddGatewayAuthentication(builder.Configuration);
    builder.Services.AddGatewayRateLimiting(builder.Configuration);
    builder.Services.AddGatewayHealthChecks(builder.Configuration);

    builder.Services.AddHttpClient();
    builder.Services.AddEndpointsApiExplorer();

    //builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseSerilogRequestLogging();

    app.UseCors("GatewayPolicy");

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    //app.MapControllers();
    app.UseRateLimiter();
    app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration.TotalMilliseconds
                }),
                timestamp = DateTime.UtcNow
            });
            await context.Response.WriteAsync(result);
        }
    });

    // Gateway info endpoint
    app.MapGet("/gateway/info", () => Results.Ok(new
    {
        name = "API Gateway",
        version = "1.0.0",
        timestamp = DateTime.UtcNow
    }));

    // YARP Reverse Proxy
    app.MapReverseProxy();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "ApiGateway Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

