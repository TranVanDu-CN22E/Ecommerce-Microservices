using CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("Default")));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
