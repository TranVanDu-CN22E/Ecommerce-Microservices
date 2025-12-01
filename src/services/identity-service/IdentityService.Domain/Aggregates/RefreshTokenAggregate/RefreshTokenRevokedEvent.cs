using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.RefreshTokenAggregate
{
    public sealed class RefreshTokenRevokedEvent : IDomainEvent
    {
        public Guid Token { get;}
        public string Reason { get;}
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public RefreshTokenRevokedEvent(Guid token, string reason)
        {
            Token = token;
            Reason = reason;
        }
    }
}
