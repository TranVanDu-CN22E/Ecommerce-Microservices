namespace CatalogShared.Models
{
    public sealed record ReserveStockRequest(
        string OrderId,
        string ProductVariantId,
        int Quantity,
        DateTime ReserveTime
    );
}
