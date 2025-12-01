using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class UserAddress : AggregateRoot<Guid>
    {
        public UserId UserId { get; private set; } = default!;
        public string Address { get; private set; } = string.Empty;
        public string Note { get; private set; } = string.Empty;
    }
}
