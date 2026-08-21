namespace CatalogService.Application.DTOs
{
    public sealed record OutboxMessageDto(string Type, string Content, DateTime OccurredOnUtc);
}
