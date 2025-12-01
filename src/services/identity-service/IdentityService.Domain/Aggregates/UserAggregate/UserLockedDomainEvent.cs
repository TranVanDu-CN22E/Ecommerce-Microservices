using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class UserLockedDomainEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public string Reason { get; }
        public DateTime? LockedUntil { get; }

        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public UserLockedDomainEvent(Guid userId, string reason, DateTime? lockedUntil)
        {
            UserId = userId;
            Reason = reason;
            LockedUntil = lockedUntil;
        }
    }
}
