namespace CatalogService.Application.Common
{
    public static class ProductErrors
    {
        public static readonly Error ProductIdRequired = new("Product.IdRequired", "Product id is required.");

        public static readonly Error ProductNameRequired = new("Product.NameRequired", "Product name is required.");
        public static readonly Error ProductNameMaxLength = new("Product.NameMaxLength", "Product name must not exceed 200 characters.");

        public static readonly Error ProductSlugRequired = new("Product.SlugReuired", "Product slug is required.");
        public static readonly Error ProductSlugInvalid = new("Product.SlugInvalid", "Slug must be lowercase and contain only letters, numbers, and hyphens.");
        public static readonly Error ProductSlugMaxLength = new("Product.SlugMaxLength", "Slug must not exceed 200 characters.");

        public static readonly Error ProductThumbnailRequired = new("Product.ThumbnailRequired", "Thumbnail is requerid.");

        public static readonly Error VariantSkuRequired = new("Variant.SkuRequired", "SKU is required.");
        public static readonly Error VariantSkuMaxLenth = new("Variant.SkuMaxLength", "SKU must not exceed 50 characters.");
        public static readonly Error VariantSkuInvalid = new("Variant.SkuInvalid", "SKU must contain only uppercase letters, numbers, and hyphens.");

        public static readonly Error VariantPriceGreaterThanZero = new("Variant.PriceGreaterThanZero", "Price must be greater than 0.");

        public static readonly Error VariantCurrencyRequired = new("Variant.CurrencyRequired", "Currency is required.");
        public static readonly Error VariantCurrencyMaxLength = new("Variant.CurrrencyMaxLength", "Currency code must be 3 characters (e.g., VND, USD).");
        public static readonly Error VariantCurrencyLetter = new("Variant.CurrencyLetter", "Currency must contain only letters.");

        public static readonly Error VariantOriginalPriceGreaterThanOrEqualToZero = new("Variant.OriginalPrice", "Original price must be non-negative.");

        public static readonly Error VariantOriginalPriceGreterThanOrEqualToSellingPrice = new("Variant.OriginalPriceAndPrice", "Original price must be greater than or equal to the selling price.");
    }
}
