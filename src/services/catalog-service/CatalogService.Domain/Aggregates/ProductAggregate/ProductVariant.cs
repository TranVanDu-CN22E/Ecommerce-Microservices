using CatalogService.Domain.Aggregates.ProductAggregate;

public sealed class ProductVariant
{
    private readonly List<VariantAttribute> _attributes = new();

    public ProductVariantId ProductVariantId { get; private set; }
    public ProductSku ProductSku { get; private set; } // Unique identifier for the variant, e.g., "RED-MEDIUM"
    public Money Price { get; private set; }
    public Money? OriginalPrice { get; private set; }
    public int StockQuantity { get; set; }
    public IReadOnlyCollection<VariantAttribute> Attributes => _attributes.AsReadOnly();
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ProductVariant() { }

    internal ProductVariant(
        ProductVariantId id,
        ProductSku sku,
        Money price,
        Money? originalPrice,
        IEnumerable<VariantAttribute> attributes,
        string? imageUrl,
        bool isActive)
    {
        if (price.Amount <= 0)
            throw new ArgumentException("Price must be greater than zero");

        ProductVariantId = id;
        ProductSku = sku;
        Price = price;
        OriginalPrice = originalPrice;
        _attributes.AddRange(attributes);
        ImageUrl = imageUrl;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
    }

    internal void UpdatePrice(Money? newPrice, Money? newOriginalPrice)
    {
        if (newPrice is null)
            throw new ArgumentNullException(nameof(newPrice), "Price must not be null");

        if (newPrice.Amount <= 0)
            throw new ArgumentException("Price must be greater than zero");

        Price = newPrice;
        OriginalPrice = newOriginalPrice;
    }
    public void Update(Money? newPrice, Money? newOriginalPrice = null, List<VariantAttribute> variantAttributes = null,
    string? imageUrl = null)
    {
        if (newPrice is null)
            throw new ArgumentNullException(nameof(newPrice), "Price must not be null");

        if (newPrice.Amount <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(newPrice));

        Price = newPrice;

        if (newOriginalPrice is not null)
        {
            OriginalPrice = newOriginalPrice;
        }

        _attributes.Clear();
        if (variantAttributes is not null)
        {
            _attributes.AddRange(variantAttributes);
        }

        ImageUrl = imageUrl ?? ImageUrl; // Only update the image URL if a new one is provided
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    public bool IsOnSale =>
        OriginalPrice is Money original &&
        original.Currency == Price.Currency &&
        original.Amount > Price.Amount;
}
