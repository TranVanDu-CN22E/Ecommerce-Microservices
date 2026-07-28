using CatalogService.Application.Abstractions.Messaging;
using CatalogService.Application.Common;

namespace CatalogService.Application.Features.CatalogFeatures.Commands.DeleteCategory
{
    public sealed record DeleteCategoryCommand(string CategoryId) : ICommand<Result<string>>;
}
