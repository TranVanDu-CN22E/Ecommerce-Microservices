using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.RefreshTokenAggregate
{
    public sealed class RefreshTokenRevokedEvent : IDomainEvent
    {
        public Guid Token { get;}
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public RefreshTokenRevokedEvent(Guid token)
        {
            Token = token;
        }
    }
}
