namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public sealed class PaymentMethod
    {
        /*      phải đổi sealed thành abstract để có thể kế thừa
                public static readonly PaymentMethod CreditCard = new CreditCardPaymentMethod(); // thẻ tín dụng
                public static readonly PaymentMethod Cash = new CashPaymentMethod(); // tiền mặt
                public static readonly PaymentMethod BankTransfer = new BankTransferPaymentMethod(); // chuyển khoản ngân hàng*/
        public static readonly PaymentMethod CreditCard = new PaymentMethod(1, "CreditCard"); // thẻ tín dụng
        public static readonly PaymentMethod Cash = new PaymentMethod(2, "Cash"); // tiền mặt
        public static readonly PaymentMethod BankTransfer = new PaymentMethod(3, "BankTransfer"); // chuyển khoản ngân hàng

        public int Id { get; private set; }
        public string Name { get; private set; }
        protected PaymentMethod(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public static PaymentMethod FromId(int id)
        {
            return id switch
            {
                1 => CreditCard,
                2 => Cash,
                3 => BankTransfer,
                _ => throw new ArgumentException($"Invalid payment method id: {id}")
            };
        }
    }
/*  public class CreditCardPaymentMethod : PaymentMethod
    {
        public CreditCardPaymentMethod() : base(1, "CreditCard") { }
    }
    public class CashPaymentMethod : PaymentMethod {
        public CashPaymentMethod(): base(2, "Cash") { }
    }
    public class BankTransferPaymentMethod : PaymentMethod {
        public BankTransferPaymentMethod() : base(3, "BankTransfer") { }
    }*/
}
/*
 modelBuilder.Entity<Order>(builder =>
        {
            builder.HasKey(o => o.Id);

            // CẤU HÌNH SMART ENUM PAYMENT METHOD Ở ĐÂY:
            builder.Property(o => o.PaymentMethod)
                .HasConversion(
                    // 1. Khi LƯU: Chuyển đối tượng PaymentMethod thành số int (lấy trường Id)
                    method => method.Id, 
                    
                    // 2. Khi ĐỌC: Lấy số int từ Database lên, chạy hàm FromId để dịch ngược thành đối tượng
                    id => PaymentMethod.FromId(id) 
                )
                .HasColumnName("PaymentMethodId") // Đặt tên cột dưới DB là PaymentMethodId
                .IsRequired(); // Bắt buộc phải có
        });
*/