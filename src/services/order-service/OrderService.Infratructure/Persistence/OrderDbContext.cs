using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Aggregates.ShippingRuleAggregate;

namespace OrderService.Infratructure.Persistence
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<ShippingRule> ShippingRules => Set<ShippingRule>();
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
