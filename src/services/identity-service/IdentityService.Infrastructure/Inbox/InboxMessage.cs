namespace IdentityService.Infrastructure.Inbox
{
    public class InboxMessage
    {
        public Guid Id { get; set; }
        public string MessageId { get; set; } = "";
        public string Payload { get; set; } = "";
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
        public bool Processed { get; set; }
    }
}
