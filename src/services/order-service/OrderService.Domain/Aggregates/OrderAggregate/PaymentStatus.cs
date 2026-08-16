namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public readonly record struct PaymentStatus
    {
        public static readonly PaymentStatus Unpaid = new PaymentStatus(1, "Unpaid"); // chưa thanh toán
        public static readonly PaymentStatus Paid = new PaymentStatus(2, "Paid"); // đã thanh toán
        public static readonly PaymentStatus Refunded = new PaymentStatus(3, "Refunded"); // đã hoàn trã
        public int Id { get; init; }
        public string Name { get; init; }
        private PaymentStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public static PaymentStatus FromId(int id)
        {
            return id switch
            {
                1 => Unpaid,
                2 => Paid,
                3 => Refunded,
                _ => throw new ArgumentException($"Invalid payment status id: {id}")
            };
        }
    }
}
