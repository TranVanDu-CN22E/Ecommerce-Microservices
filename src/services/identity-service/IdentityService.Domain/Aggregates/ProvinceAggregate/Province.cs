using IdentityService.Domain.Aggregates.UserAggregate;
using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.ProvinceAggregate
{
    public sealed class Province : AggregateRoot<ProvinceId>
    {
        public string ProvinceName { get; private set; } = string.Empty;
        public List<UserAddress> UserAddresses { get; private set; }
        private Province() { }
        private Province(ProvinceId id, string name)
        {
            Id = id;
            ProvinceName = name;
        }
        public static Province Create(int id, string name)
        {
            return new Province(ProvinceId.Create(id), name);
        }
    }
}
