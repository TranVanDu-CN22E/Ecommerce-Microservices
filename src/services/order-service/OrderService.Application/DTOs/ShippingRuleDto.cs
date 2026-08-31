namespace OrderService.Application.DTOs
{
    public sealed record ShippingRuleDto
    {
        public string Id { get; set; }
        public string Province { get; set; }
        public Money BaseFee { get; set; }
        public Money? FreeThreshold { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
