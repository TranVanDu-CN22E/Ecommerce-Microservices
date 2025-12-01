using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class UserCreatedDomainEvent : IDomainEvent
    {
        public Guid UserId { get; }
        public string Email { get; }

        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public UserCreatedDomainEvent(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}
