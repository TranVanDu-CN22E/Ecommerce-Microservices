using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Persistence
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
        public DbSet<VariantAttribute> VariantAttributes => Set<VariantAttribute>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
        //public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
