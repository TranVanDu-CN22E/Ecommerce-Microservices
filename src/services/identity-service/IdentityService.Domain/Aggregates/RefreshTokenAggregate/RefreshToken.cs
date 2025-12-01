using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.RefreshTokenAggregate
{
    public sealed class RefreshToken : AggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }
        private RefreshToken() { }
        private RefreshToken(RefreshTokenId refreshTokenId, Guid userId, string token, DateTime expiresAt)
        {
            Id = refreshTokenId.Value;
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
        }
        public static RefreshToken Create (Guid userId, string token, DateTime expiresAt)
            => new (RefreshTokenId.New(), userId, token, expiresAt );
        public void Revoke(string reason)
        {
            IsRevoked = true;
            AddDomainEvent(new RefreshTokenRevokedEvent(Id, reason));
        }
    }
}
