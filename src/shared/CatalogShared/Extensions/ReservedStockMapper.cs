using CatalogShared.Models;
using Google.Protobuf.WellKnownTypes;

namespace CatalogShared.Extensions
{
    public static class ReservedStockMapper
    {
        // Proto → Shared Model (dùng phía Server)
        public static ReserveStockRequest ToModel(this Protos.ReserveStockRequest proto) =>
            new(
                OrderId: proto.OrderId,
                ProductVariantId: proto.ProductVariantId,
                Quantity: proto.Quantity,
                ReserveTime: proto.ReserveTime.ToDateTime()
            );

        // Shared Model → Proto Response (dùng phía Server)
        public static Protos.ReserveStockResponse ToProto(this ReserveStockResult result) =>
            new()
            {
                Success = result.Success,
                Errors = { result.Errors },
                ReservedQuantity = result.ReservedQuantity,
                AvailableStock = result.AvailableStock,
                IdempotencyKey = result.IdempotencyKey
            };// Shared Model → Proto Request (dùng phía Client - OrderService/PaymentService)
        public static Protos.ReserveStockRequest ToProto(this ReserveStockRequest model) =>
            new()
            {
                OrderId = model.OrderId,
                ProductVariantId = model.ProductVariantId,
                Quantity = model.Quantity,
                ReserveTime = Timestamp.FromDateTime(model.ReserveTime.ToUniversalTime())
            };
    }
}