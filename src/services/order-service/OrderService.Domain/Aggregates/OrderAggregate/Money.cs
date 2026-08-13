namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public readonly record struct Money (decimal Amount, string Currency)
    {
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
