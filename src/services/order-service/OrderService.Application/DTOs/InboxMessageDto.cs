namespace OrderService.Application.DTOs
{
    public sealed record InboxMessageDto(
        Guid Id,
        string Type,
        string Content,
        DateTime ReceivedOnUtc
    );
}
