using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class UserBannedDomainEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public string Reason { get; }
        public DateTime? BannedUntil { get; }
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public UserBannedDomainEvent(Guid userId, string reason, DateTime? bannedUntil)
        {
            UserId = userId;
            Reason = reason;
            BannedUntil = bannedUntil;
        }
    }
}
