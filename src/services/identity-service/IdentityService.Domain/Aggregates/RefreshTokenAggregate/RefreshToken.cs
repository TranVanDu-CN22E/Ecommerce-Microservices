using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.RefreshTokenAggregate
{
    public sealed class RefreshToken : AggregateRoot<Guid>
    {
        public UserId UserId { get; private set; }
        public User User { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        private RefreshToken() { }
        private RefreshToken(RefreshTokenId refreshTokenId, UserId userId ,string token, DateTime expiresAt)
        {
            Id = refreshTokenId.Value;
            UserId = userId;
            Token = token ?? string.Empty;
            ExpiresAt = expiresAt;
        }
        public static RefreshToken Create (UserId userId, string token, DateTime expiresAt)
            => new (RefreshTokenId.New(), userId, token, expiresAt );
        public void Revoke()
        {
            if (IsRevoked)
                return;

            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;

            AddDomainEvent(new RefreshTokenRevokedEvent(Id));
        }

        public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;
    }
}
