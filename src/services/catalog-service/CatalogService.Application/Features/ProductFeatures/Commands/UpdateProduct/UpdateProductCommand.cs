using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;
using CatalogService.Application.DTOs;

namespace CatalogService.Application.Features.ProductFeatures.Commands.UpdateProduct
{
    public sealed record UpdateProductCommand : ICommand<Result<bool>>
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string CategoryId { get; init; } = string.Empty;
        public IFormFile Thumbnail { get; init; } = null!;
        public List<CreateProductVariantDto> Variants { get; init; } = new();
    }
}
