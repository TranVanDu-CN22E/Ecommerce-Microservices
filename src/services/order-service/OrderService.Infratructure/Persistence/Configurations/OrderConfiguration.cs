using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Aggregates.OrderAggregate;
using System.Net.NetworkInformation;

namespace OrderService.Infratructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => new OrderId(value))
                .HasColumnName("OrderId")
                .IsRequired();
            builder.Property(o => o.CustomerId)
                .HasConversion(
                    id => id.Value,
                    value => new CustomerId(value))
                .HasColumnName("CustomerId")
                .IsRequired();
            builder.Property(o => o.OrderStatus)
                .HasConversion(
                    status => status.Id,
                    id => OrderStatus.FromId(id))
                .HasColumnName("OrderStatus")
                .IsRequired();
            builder.Property(o => o.PaymentMethod)
                .HasConversion(
                    method => method.Id,
                    id => PaymentMethod.FromId(id)
                ).HasColumnName("PaymentMethod").IsRequired();
            builder.Property(o => o.PaymentStatus).HasConversion(status => status.Id, id => PaymentStatus.FromId(id)).HasColumnName("PaymentStatus").IsRequired();

            builder.OwnsOne(s => s.ShippingAddress, address =>
            {
                address.Property(a => a.OrderId).ValueGeneratedNever().HasColumnName("OrderId").IsRequired();
                address.Property(a => a.RecipientName).HasColumnName("RecipientName").IsRequired();
                address.Property(a => a.PhoneNumber).HasColumnName("PhoneNumber").IsRequired();
                address.Property(a => a.AddressLine).HasColumnName("AddressLine").IsRequired();
                address.Property(a => a.Ward).HasColumnName("Ward").IsRequired();
                address.Property(a => a.District).HasColumnName("District").IsRequired();
                address.Property(a => a.Province).HasColumnName("Province").IsRequired();
                address.Property(a => a.Country).HasColumnName("Country").IsRequired();
            });
            builder.OwnsOne(o => o.ShippingFee, fee =>
            {
                fee.Property(f => f.Amount).HasColumnName("Amount").IsRequired();
                fee.Property(f => f.Currency).HasColumnName("Currency").IsRequired();
            });
            builder.Property(o => o.Note).HasColumnName("Note");
            builder.Property(o => o.CancellationReason).HasColumnName("CancellationReason");
            builder.Property(o => o.CreatedAt).HasColumnName("CreatedAt").IsRequired();
            builder.Property(o => o.ConfirmedAt).HasColumnName("ConfirmedAt");
            builder.Property(o => o.CancelledAt).HasColumnName("CancelledAt");
            builder.Property(o => o.PaidAt).HasColumnName("PaidAt");

            
        }
    }
}
