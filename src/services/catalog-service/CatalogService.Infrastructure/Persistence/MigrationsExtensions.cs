using Microsoft.EntityFrameworkCore;


namespace CatalogService.Infrastructure.Persistence
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = services
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("EF.Migrations");

            var db = services.GetRequiredService<CatalogDbContext>();

            const int maxRetry = 10;
            var delay = TimeSpan.FromSeconds(3);

            for (var attempt = 1; attempt <= maxRetry; attempt++)
            {
                try
                {
                    logger.LogInformation("Applying EF Core migrations (attempt {Attempt}/{Max})", attempt, maxRetry);
                    db.Database.Migrate();
                    logger.LogInformation("EF Core migrations applied successfully.");
                    return;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Database not ready, retrying...");
                    Thread.Sleep(delay);
                }
            }

            throw new Exception("Database migration failed after multiple retries.");

        }
    }
}
