namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public sealed class VariantAttribute
    {
        public Guid ProductVariantAttributeId { get; private set; } = Guid.NewGuid();
        public ProductVariantId ProductVariantId { get; private set; }
        public string Name { get; set; }
        public string Value { get; set; }
        private VariantAttribute() { }
        public VariantAttribute(ProductVariantId productVariantId, string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            ProductVariantId = productVariantId;
            Name = name;
            Value = value;

        }
        public static VariantAttribute Create(ProductVariantId productVariantId, string name, string value)
        {
            return new VariantAttribute(productVariantId, name, value);
        }
        public VariantAttribute Update(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            Name = name;
            Value = value;
            return this;
        }
    }
}
/*
    { Name = "Color", Value = "Red" },
    { Name = "Size", Value = "M" }
*/