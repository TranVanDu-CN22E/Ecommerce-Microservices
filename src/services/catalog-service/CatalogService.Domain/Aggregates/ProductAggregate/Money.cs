namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public sealed record Money
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; }
        private Money() { }
        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
        public static Money Create(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be null or whitespace.", nameof(currency));
            if (currency.Length != 3)
                throw new ArgumentException("Currency must be a 3-letter ISO code.", nameof(currency));
            return new Money(amount, currency.ToUpperInvariant().Trim());
        }
        public static Money Zero(string currency = "VND") => Create(0, currency);
    }
}
