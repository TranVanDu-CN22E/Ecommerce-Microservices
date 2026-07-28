using CatalogService.Application.Common;
using CatalogService.Application.DTOs;
using CatalogService.Application.Abstractions.Messaging;

namespace CatalogService.Application.Features.ProductFeatures.Queries.GetProductById
{
    public sealed record GetProductByIdQuery : IQuery<Result<ProductResponseDto>>
    {
        public string ProductId { get; init; } = string.Empty;
    }
}
