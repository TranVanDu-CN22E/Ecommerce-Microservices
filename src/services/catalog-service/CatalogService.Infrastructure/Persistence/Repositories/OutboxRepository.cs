using CatalogService.Application.Abstractions.Services;
using CatalogService.Application.DTOs;
using CatalogService.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Persistence.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private CatalogDbContext _context;
        public OutboxRepository(CatalogDbContext context) { _context = context; }
        public async Task AddAsync(OutboxMessageDto message, CancellationToken ct = default)
        {
            var entity = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = message.Type,
                Content = message.Content,
                OccurredOnUtc = message.OccurredOnUtc
            };

            await _context.OutboxMessages.AddAsync(entity, ct);
        }
    }
}
