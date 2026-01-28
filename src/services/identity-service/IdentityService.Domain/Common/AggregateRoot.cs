using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace IdentityService.Domain.Common
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>
    {
    }
}
