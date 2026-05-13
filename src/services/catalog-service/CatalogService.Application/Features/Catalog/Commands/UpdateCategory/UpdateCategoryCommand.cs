using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;

namespace CatalogService.Application.Features.Catalog.Commands.UpdateCategory
{
    public sealed record UpdateCategoryCommand(
        string Id,
        string? Name,
        string? Slug,
        string? ParentId,
        string? Description,
        int? DisplayOrder)
        : ICommand<Result>;
}
