namespace CatalogShared.Models
{
    public record ReserveStockResult(
        bool Success,
        IReadOnlyList<string> Errors,
        int ReservedQuantity,
        int AvailableStock,
        string IdempotencyKey
    );
}
