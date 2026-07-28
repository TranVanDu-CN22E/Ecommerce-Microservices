using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;
using CatalogService.Application.DTOs;

namespace CatalogService.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public sealed record CreateProductCommand : ICommand<Result<Guid>>
    {
        public string Name { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string CategoryId { get; init; } = string.Empty;
        public IFormFile Thumbnail { get; init; }
        public List<IFormFile> Images {  get; init; }
        public List<CreateProductVariantDto>? Variants { get; init; }
    }
}
