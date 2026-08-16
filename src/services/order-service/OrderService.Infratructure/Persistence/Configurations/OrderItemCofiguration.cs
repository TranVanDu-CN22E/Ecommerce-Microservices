using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Aggregates.OrderAggregate;

namespace OrderService.Infratructure.Persistence.Configurations
{
    public class OrderItemCofiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(i => i.OrderItemId);

            builder.Property(i => i.OrderItemId).ValueGeneratedNever().HasColumnName("OrderItemId").IsRequired();
            builder.Property(i => i.ProductId).HasColumnName("ProductId").IsRequired();
            builder.Property(i => i.ProductVariantId).HasColumnName("ProductVariantId").IsRequired();
            builder.Property(i => i.ProductName).HasColumnName("ProductName").IsRequired();
            builder.Property(i => i.VariantSku).HasColumnName("VariantSku").IsRequired();
            builder.Property(i => i.VariantAttribute).HasColumnName("VariantAttribute").HasMaxLength(255);
            builder.Property(i => i.UnitPrice).HasColumnName("UnitPrice").IsRequired();
            builder.Property(i => i.Quantity).HasColumnName("Quantity").IsRequired();
            builder.OwnsOne(i => i.SubTotal, subTotal =>
            {
                subTotal.Property(i => i.Amount).HasColumnName("Amount").IsRequired();
                subTotal.Property(i => i.Currency).HasColumnName("Currency").IsRequired().HasMaxLength(3);
            });

            builder.HasOne<Order>().WithMany(o => o.Items).HasForeignKey(o => o.OrderId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}