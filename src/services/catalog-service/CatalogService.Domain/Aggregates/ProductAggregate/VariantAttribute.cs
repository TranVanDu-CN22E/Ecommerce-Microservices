using System.Xml.Linq;

namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public sealed record VariantAttribute()
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int StockQuantity { get; private set; }
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