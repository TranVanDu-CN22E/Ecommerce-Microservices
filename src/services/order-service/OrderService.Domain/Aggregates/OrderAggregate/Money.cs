namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public sealed record Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = default!;
        private Money() { }
        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
        public static Money Create(decimal Amount, string Currency)
        {
            if (Amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative.", nameof(Amount));
            }
            if (string.IsNullOrWhiteSpace(Currency))
            {
                throw new ArgumentException("Currency cannot be null or whitespace.", nameof(Currency));
            }
            return new Money(Amount, Currency);
        }
        public static Money Zero(string Currency = "VND")
        {
            return new Money(0, Currency);
        }
        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw new ArgumentException("Currency mismatch.");
            }
            return new Money(left.Amount + right.Amount, left.Currency);
        }
        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw new ArgumentException("Currency mismatch.");
            }
            return new Money(left.Amount - right.Amount, left.Currency);
        }
        public static Money operator *(Money money, decimal multiplier)
        {
            return new Money(money.Amount * multiplier, money.Currency);
        }
    }
}
