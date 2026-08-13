namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public readonly record struct OrderStatus
    {
        public static readonly OrderStatus Pending = new OrderStatus(1, "Pending"); // đang chờ xử lý
        public static readonly OrderStatus Completed = new OrderStatus(2, "Completed"); // đã hoàn thành
        public static readonly OrderStatus Cancelled = new OrderStatus(3, "Cancelled"); // đã hủy
        public int Id { get; init; }
        public string Name { get; init; }
        private OrderStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public static OrderStatus FromId(int id)
        {
            return id switch
            {
                1 => Pending,
                2 => Completed,
                3 => Cancelled,
                _ => throw new ArgumentException($"Invalid order status id: {id}")
            };
        }
    }
}
