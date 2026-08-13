namespace OrderService.Domain.Aggregates.ShippingRuleAggregate
{
    public readonly record struct Province (string Value)
    {
        public static Province Create(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Province cannot be empty.");
            }
            if (value.Length > 100)
                throw new ArgumentException("Province cannot exceed 100 characters.");
            return new Province(value.Trim());
        }
    }
}
