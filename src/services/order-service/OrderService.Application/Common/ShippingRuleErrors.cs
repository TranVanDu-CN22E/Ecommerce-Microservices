namespace OrderService.Application.Common
{
    public static class ShippingRuleErrors
    {
        public static readonly Error FileIsNotNull = new Error("ShippingRule.FileIsNotNull", "Shipping rule file cannot be null.");
        public static readonly Error ShippingRuleNotFound = new Error("ShippingRule.ShippingRuleNotFound", "Shipping rule not found.");
    }
}
