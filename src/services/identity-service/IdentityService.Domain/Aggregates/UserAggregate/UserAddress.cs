using IdentityService.Domain.Aggregates.ProvinceAggregate;
using IdentityService.Domain.Common;

namespace IdentityService.Domain.Aggregates.UserAggregate
{
    public sealed class UserAddress : AggregateRoot<UserAddressId>
    {
        public UserId UserId { get; private set; } = default!;
        public User User { get; private set; } = default!;
        public string Address { get; private set; } = string.Empty;
        public string Note { get; private set; } = string.Empty;
        public UserPhone Phone { get; private set; }
        public ProvinceId ProvinceId { get; private set; } = default!;
        public Province Province { get; private set; } = default!;
        private UserAddress() { }
        private UserAddress(UserAddressId userAddressId, UserId userId, string address, string note, UserPhone phone, ProvinceId provinceId)
        {
            Id = userAddressId;
            UserId = userId;
            Address = address;
            Note = note;
            Phone = phone;
            ProvinceId = provinceId;
        }
        public static UserAddress Create(UserId userId, string address, string? note, UserPhone phone, ProvinceId provinceId)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address cannot be empty", nameof(address));
            return new UserAddress(UserAddressId.New(), userId, address, note??string.Empty, phone, provinceId);
        }
    }
}
