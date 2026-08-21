using CatalogService.Application.DTOs;

namespace CatalogService.Application.Abstractions.Services
{
    public interface IOutboxRepository
    {
        Task AddAsync(OutboxMessageDto message, CancellationToken ct = default);
    }
}
