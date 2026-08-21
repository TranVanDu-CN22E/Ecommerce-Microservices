using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Abstractions.Services;
using CatalogService.Application.Common;
using CatalogService.Application.DTOs;
using CatalogShared.Models;
using MediatR;
using System.Text.Json;

namespace CatalogService.Application.Features.OrderFeatures.Commands.PlaceOrder
{
    internal sealed class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, Result<Guid>>
    {
        private readonly IProductCacheService _cacheService;
        private readonly IOutboxRepository _outboxRepository;

        public PlaceOrderCommandHandler(IProductCacheService cacheService, IOutboxRepository outboxRepository)
        {
            _cacheService = cacheService;
            _outboxRepository = outboxRepository;
        }

        public async Task<Result<Guid>> Handle(PlaceOrderCommand request, CancellationToken ct)
        {
            var @event = request.Event;
            var reservedItems = new List<(string ProductId, string VariantId, int Quantity)>();
            try
            {
                // Reserve stock trên Redis
                foreach (var item in @event.OrderItems)
                {
                    if (!await _cacheService.TryReserveStockAsync(
                        item.ProductId, item.ProductVariantId, item.Quantity))
                    {
                        // Release items đã reserve trước đó
                        foreach (var (pid, vid, qty) in reservedItems)
                            await _cacheService.ReleaseReservedAsync(pid, vid, qty);

                        return Result<Guid>.Failure(new[] {
                        new Error("ProductKafka", $"Insufficient stock: {item.ProductName}")
                    });
                    }

                    reservedItems.Add((item.ProductId, item.ProductVariantId, item.Quantity));
                }
                // Lưu Outbox - TransactionBehavior tự wrap Begin/Commit/Rollback
                await _outboxRepository.AddAsync(new OutboxMessageDto(
                    nameof(OrderPlaceEvent),
                    JsonSerializer.Serialize(@event),
                    DateTime.UtcNow), ct);

                // TransactionBehavior.CommitTransactionAsync() đã gọi SaveChanges bên trong

                return Result<Guid>.Success(@event.OrderId);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // TransactionBehavior tự Rollback DB
                // Ta chỉ cần compensating Redis
                foreach (var (pid, vid, qty) in reservedItems)
                    await _cacheService.ReleaseReservedAsync(pid, vid, qty);

                return Result<Guid>.Failure(new[] {
                new Error("PlaceOrder.DbError", "Failed to place order. Reserved stock released.")
            });
            }           
        }
    }
}
