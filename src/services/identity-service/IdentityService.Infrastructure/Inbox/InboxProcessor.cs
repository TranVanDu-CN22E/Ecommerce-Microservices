using IdentityService.Infrastructure.Persistence;
using Medo;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Inbox
{
    public class InboxProcessor
    {
        private readonly IdentityDbContext _db;

        public InboxProcessor(IdentityDbContext db)
        {
            _db = db;
        }

        public async Task ProcessAsync(string messageId, string payload, CancellationToken ct)
        {
            var exists = await _db.InboxMessages.AnyAsync(x => x.MessageId == messageId, ct);
            if (exists) return; // idempotency

            var msg = new InboxMessage
            {
                Id = Uuid7.NewUuid7(),
                MessageId = messageId,
                Payload = payload,
                Processed = true
            };

            await _db.InboxMessages.AddAsync(msg, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}
