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

            // Tác dụng: Tăng tốc truy vấn "Lấy lịch sử đơn hàng của tôi" của Khách hàng.
            builder.HasIndex(o => o.CustomerId)
                   .HasDatabaseName("IX_Orders_CustomerId");

            // Tác dụng: Tối ưu cho trang Lịch sử đơn hàng khi cần Sắp xếp (ORDER BY CreatedAt DESC) và Phân trang.
            builder.HasIndex(o => new { o.CustomerId, o.CreatedAt })
                   .HasDatabaseName("IX_Orders_CustomerId_CreatedAt");

            // Tác dụng: Tối ưu cho các câu lệnh thống kê, báo cáo doanh thu theo ngày/tháng/năm của Admin.
            builder.HasIndex(o => o.CreatedAt)
                   .HasDatabaseName("IX_Orders_CreatedAt");

            // Tác dụng: Admin thường xuyên lọc đơn "Chờ thanh toán" 
            // Giả sử id = 1 là 'Chờ thanh toán'. Chúng ta chỉ index các đơn này để index siêu nhẹ.
            builder.HasIndex(o => o.OrderStatus)
                   .HasFilter("\"OrderStatus\" = 1")
                   .HasDatabaseName("IX_Orders_OrderStatus_Pending_Partial");

            // Tác dụng: Chỉ lưu các đơn bị hủy. Giúp thống kê lý do hủy nhanh chóng.
            builder.HasIndex(o => o.CancelledAt)
                   .HasFilter("\"CancelledAt\" IS NOT NULL")
                   .HasDatabaseName("IX_Orders_CancelledAt_Partial");

            builder.HasIndex(o => o.PaymentStatus)
                   .HasFilter("\"PaymentStatus\" = 1") // Chỉ lưu các đơn có mã là 1
                   .HasDatabaseName("IX_Orders_PaymentStatus_Unpaid_Partial");

            // Partial Index cho các đơn đã HOÀN TIỀN (PaymentStatus = 3 - Refunded)
            builder.HasIndex(o => o.PaymentStatus)
                   .HasFilter("\"PaymentStatus\" = 3") // Chỉ lưu các đơn có mã là 3 (Refunded)
                   .HasDatabaseName("IX_Orders_PaymentStatus_Refunded_Partial");
        }
    }
}
