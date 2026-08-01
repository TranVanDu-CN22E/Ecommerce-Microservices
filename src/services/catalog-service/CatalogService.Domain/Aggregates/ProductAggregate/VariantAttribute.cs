using System.Xml.Linq;

namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public sealed class VariantAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int StockQuantity { get; private set; } = 0;
        public int SoldQuantity { get; private set; } = 0;
        private VariantAttribute(){}
        public VariantAttribute(string name, string value, int? stockQuantity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
            if (!stockQuantity.HasValue || stockQuantity < 0)
            {
                throw new ArgumentException("Stock quantity cannot be negative.", nameof(stockQuantity));
            }
            else
            {
                StockQuantity = stockQuantity.Value;
            }
            Name = name;
            Value = value;
            
        }
        public void IncreaseSoldQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException();
            SoldQuantity += quantity;
        }
        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException();

            StockQuantity += quantity;
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException();

            if (StockQuantity < quantity)
                throw new InvalidOperationException("Insufficient stock.");

            StockQuantity -= quantity;
        }

        public void SetStock(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException();

            StockQuantity = quantity;
        }
    }
}
/*
    { Name = "Color", Value = "Red" },
    { Name = "Size", Value = "M" }
*/