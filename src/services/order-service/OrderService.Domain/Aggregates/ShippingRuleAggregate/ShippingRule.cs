using OrderService.Domain.Aggregates.OrderAggregate;
using OrderService.Domain.Common;

namespace OrderService.Domain.Aggregates.ShippingRuleAggregate
{
    public class ShippingRule: AggregateRoot<ShippingRuleId>
    {
        public Province Province { get; private set; } = default!;

        /// <summary>Phí ship cơ bản khi đơn chưa đạt ngưỡng miễn phí.</summary>
        public Money BaseFee { get; private set; } = default!;

        /// <summary>Giá trị đơn hàng tối thiểu để được miễn phí ship. Null = không có miễn phí.</summary>
        public Money? FreeThreshold { get; private set; }

        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private ShippingRule() { }

        private ShippingRule(ShippingRuleId id, Province province, Money baseFee, Money? freeThreshold)
        {
            Id = id;
            Province = province;
            BaseFee = baseFee;
            FreeThreshold = freeThreshold;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public static ShippingRule Create(Province province, Money baseFee, Money? freeThreshold = null)
        {
            if (freeThreshold is not null && freeThreshold.Currency != baseFee.Currency)
                throw new ArgumentException("BaseFee and FreeThreshold must use the same currency.");

            return new ShippingRule(ShippingRuleId.New(), province, baseFee, freeThreshold);
        }

        public void Update(Money baseFee, Money? freeThreshold)
        {
            if (freeThreshold is not null && freeThreshold.Currency != baseFee.Currency)
                throw new ArgumentException("BaseFee and FreeThreshold must use the same currency.");

            BaseFee = baseFee;
            FreeThreshold = freeThreshold;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate() => IsActive = true;

        /// <summary>
        /// Tính phí ship cho đơn hàng dựa trên giá trị items.
        /// Trả về Money.Zero nếu đủ điều kiện miễn phí ship.
        /// </summary>
        public Money CalculateFee(Money orderItemsTotal)
        {
            if (orderItemsTotal.Currency != BaseFee.Currency)
                throw new InvalidOperationException("Order currency does not match shipping rule currency.");

            if (FreeThreshold is not null && orderItemsTotal.Amount >= FreeThreshold.Amount)
                return Money.Zero(BaseFee.Currency);

            return BaseFee;
        }
    }
}
