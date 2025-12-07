using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.ProvinceAggregate
{
    public sealed class Province : AggregateRoot<int>
    {
        public string ProvinceName { get; private set; } = string.Empty;
        public List<UserAddress> UserAddresses { get; private set; }
        private Province() { }
        private Province(ProvinceId id, string name)
        {
            Id = id.Value;
            ProvinceName = name;
        }
        public static Province Create(int id, string name)
        {
            return new Province(ProvinceId.Create(id), name);
        }
    }
}
