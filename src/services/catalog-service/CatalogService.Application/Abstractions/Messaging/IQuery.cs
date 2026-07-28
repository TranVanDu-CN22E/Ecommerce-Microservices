using MediatR;

namespace CatalogService.Application.Abstractions.Messaging
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
