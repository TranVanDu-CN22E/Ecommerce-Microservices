using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions.Services;
using OrderService.Application.DTOs;
using OrderService.Infratructure.Persistence.Inbox;

namespace OrderService.Infratructure.Persistence.Repository
{
    public class InboxRepository : IInboxRepository
    {
        private readonly OrderDbContext _context;
        public InboxRepository(OrderDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsProcessedAsync(Guid messageId, CancellationToken ct = default)
        {
            return await _context.Set<InboxMessage>()
                .AnyAsync(m => m.Id == messageId && m.ProcessedOnUtc != null, ct);
        }

        public async Task AddAsync(InboxMessageDto message, CancellationToken ct = default)
        {
            var entity = new InboxMessage
            {
                Id = message.Id,
                Type = message.Type,
                Content = message.Content,
                ReceivedOnUtc = message.ReceivedOnUtc
            };

            // KHÔNG gọi SaveChanges ở đây!
            // TransactionBehavior hoặc Consumer chịu trách nhiệm commit
            await _context.Set<InboxMessage>().AddAsync(entity, ct);
        }

        public async Task MarkAsProcessedAsync(Guid messageId, CancellationToken ct = default)
        {
            await _context.Set<InboxMessage>()
                .Where(m => m.Id == messageId)
                .ExecuteUpdateAsync(s => s.SetProperty(m => m.ProcessedOnUtc, DateTime.UtcNow), ct);
        }

        public async Task MarkAsFailedAsync(Guid messageId, string error, CancellationToken ct = default)
        {
            try
            {
                await _context.Set<InboxMessage>()
                    .Where(m => m.Id == messageId)
                    .ExecuteUpdateAsync(s => s.SetProperty(m => m.Error, error), ct);

                await _context.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                // Best-effort: log nhưng không throw để tránh mask original exception
                // Trong production nên dùng ILogger injected qua constructor
            }
        }
    }
}
