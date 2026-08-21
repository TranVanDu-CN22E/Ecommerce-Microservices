namespace CatalogShared.Models
{
    public record ReleaseReservedStockResult(
        bool Success,
        IReadOnlyList<string> Errors,
        int RemainingReserved,
        int AvailableStock,
        string IdempotencyKey
    );
}
