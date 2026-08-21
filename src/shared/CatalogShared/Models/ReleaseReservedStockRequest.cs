namespace CatalogShared.Models
{
    public record ReleaseReservedStockRequest(
        string IdempotencyKey,
        string ProductVariantId,
        int Quantity,
        DateTime ReleaseTime,
        string Reason
    );
}
