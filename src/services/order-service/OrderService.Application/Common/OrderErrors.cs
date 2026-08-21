namespace OrderService.Application.Common
{
    public static class OrderErrors
    {
        public static readonly Error CustomerIdRequired = new Error("CustomerId", "Customer ID is required.");
        public static readonly Error CustomerIdInvalid = new Error("CustomerId", "Customer ID is invalid.");
        public static readonly Error PaymentMethodRequired = new Error("PaymentMethod", "Payment method is required.");
        public static readonly Error PaymentMethodInvalid = new Error("PaymentMethod", "Payment method is invalid.");
        public static readonly Error RecipientNameRequired = new Error("RecipientName", "Recipient name is required.");
        public static readonly Error PhoneNumberRequired = new Error("PhoneNumber", "Phone number is required.");
        public static readonly Error PhoneNumberInvalid = new Error("PhoneNumber", "Phone number is invalid.");
        public static readonly Error AddressLineRequired = new Error("AddressLine", "Address line is required.");
        public static readonly Error WardRequired = new Error("Ward", "Ward is required.");
        public static readonly Error DistrictRequired = new Error("District", "District is required.");
        public static readonly Error ProvinceRequired = new Error("Province", "Province is required.");
        public static readonly Error CountryRequired = new Error("Country", "Country is required.");
        public static readonly Error ShippingFeeRequired = new Error("ShippingFee", "Shipping fee is required.");

        public static readonly Error AmountRequired = new Error("Amount", "Amount is required.");
        public static readonly Error CurrencyRequired = new Error("Currency", "Currency is required.");

        public static readonly Error ProductIdRequired = new Error("ProductIdRequired", "Product ID is required.");
        public static readonly Error ProductIdInvalid = new Error("ProductIdInvalid", "Product ID is invalid.");
        public static readonly Error ProductVariantIdRequired = new Error("ProductVariantIdRequired", "Product variant ID is required.");
        public static readonly Error ProductVariantIdInvalid = new Error("ProductVariantIdInvalid", "Product variant ID is invalid.");
        public static readonly Error ProductVariantOutOfStock = new Error("ProductVariantOutOfStock", "Product variant is out of stock.");
        public static readonly Error ProductNameRequired = new Error("ProductName", "Product name is required.");
        public static readonly Error VariantSkuRequired = new Error("VariantSku", "Variant SKU is required.");
        public static readonly Error UnitPriceRequired = new Error("UnitPrice", "Unit price is required.");
        public static readonly Error QuantityRequired = new Error("Quantity", "Quantity is required.");
    }
}