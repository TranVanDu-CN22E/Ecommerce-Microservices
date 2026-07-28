using CatalogService.Application.Behaviors;
using MediatR;
using FluentValidation;
using System.Reflection;

namespace CatalogService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            // MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
            });
            // FluentValidation
            services.AddValidatorsFromAssembly(assembly);
            // Pipeline Behaviors (THỨ TỰ RẤT QUAN TRỌNG)
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            services.AddAutoMapper(cfg => cfg.AddMaps(assembly));
            return services;
        }
    }
}
