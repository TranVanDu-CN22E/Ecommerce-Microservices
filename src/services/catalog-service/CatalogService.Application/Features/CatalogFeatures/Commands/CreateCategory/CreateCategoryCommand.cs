using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;

namespace CatalogService.Application.Features.CatalogFeatures.Commands.CreateCategory
{
    public sealed record CreateCategoryCommand(string Name, string Slug, string? ParentId, string? Description, int DisplayOrder) : ICommand<Result<Guid>>;
}
