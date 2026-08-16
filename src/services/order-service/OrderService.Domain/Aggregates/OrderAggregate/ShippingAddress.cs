using System.Globalization;

namespace OrderService.Domain.Aggregates.OrderAggregate
{
    public sealed record ShippingAddress
    {
        public OrderId OrderId { get; private set; }
        public string RecipientName { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string AddressLine { get; private set; } = string.Empty;
        public string Ward { get; private set; } = string.Empty;
        public string District { get; private set; } = string.Empty;
        public string Province { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;
        private ShippingAddress() { }
        private ShippingAddress(Guid orderId, string recipientName, string phoneNumber, string addressLine, string ward, string district, string province, string country)
        {
            OrderId = OrderId.Create(orderId);
            RecipientName = recipientName;
            PhoneNumber = phoneNumber;
            AddressLine = addressLine;
            Ward = ward;
            District = district;
            Province = province;
            Country = country;
        }
        public static ShippingAddress Create(Guid orderId, string recipientName, string phoneNumber, string addressLine, string ward, string district, string province, string country)
        {
            if (string.IsNullOrWhiteSpace(orderId.ToString())) throw new ArgumentNullException("Order id cannot be null or empty",nameof(orderId));
            if (string.IsNullOrWhiteSpace(recipientName))
            {
                throw new ArgumentException("Recipient name cannot be null or empty.", nameof(recipientName));
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));
            }
            if (string.IsNullOrWhiteSpace(addressLine))
            {
                throw new ArgumentException("Address line cannot be null or empty.", nameof(addressLine));
            }
            if (string.IsNullOrWhiteSpace(ward))
            {
                throw new ArgumentException("Ward cannot be null or empty.", nameof(ward));
            }
            if (string.IsNullOrWhiteSpace(district))
            {
                throw new ArgumentException("District cannot be null or empty.", nameof(district));
            }
            if (string.IsNullOrWhiteSpace(province))
            {
                throw new ArgumentException("Province cannot be null or empty.", nameof(province));
            }
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentException("Country cannot be null or empty.", nameof(country));
            }
            return new ShippingAddress(orderId, recipientName.Trim(), phoneNumber.Trim(), addressLine.Trim(), ward.Trim(), district.Trim(), province.Trim(), country.Trim());
        }
        public ShippingAddress Update(string recipientName, string phoneNumber, string addressLine, string ward, string district, string province, string country)
        {
            if (string.IsNullOrWhiteSpace(recipientName))
            {
                throw new ArgumentException("Recipient name cannot be null or empty.", nameof(recipientName));
            }
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));
            }
            if (string.IsNullOrWhiteSpace(addressLine))
            {
                throw new ArgumentException("Address line cannot be null or empty.", nameof(addressLine));
            }
            if (string.IsNullOrWhiteSpace(ward))
            {
                throw new ArgumentException("Ward cannot be null or empty.", nameof(ward));
            }
            if (string.IsNullOrWhiteSpace(district))
            {
                throw new ArgumentException("District cannot be null or empty.", nameof(district));
            }
            if (string.IsNullOrWhiteSpace(province))
            {
                throw new ArgumentException("Province cannot be null or empty.", nameof(province));
            }
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentException("Country cannot be null or empty.", nameof(country));
            }
            return this with
            {
                RecipientName = recipientName.Trim(),
                PhoneNumber = phoneNumber.Trim(),
                AddressLine = addressLine.Trim(),
                Ward = ward.Trim(),
                District = district.Trim(),
                Province = province.Trim(),
                Country = country.Trim()
            };
        }
        public override string ToString()
        {
            return $"{RecipientName}, {PhoneNumber}, {AddressLine}, {Ward}, {District}, {Province}, {Country}";
        }

    }
}
