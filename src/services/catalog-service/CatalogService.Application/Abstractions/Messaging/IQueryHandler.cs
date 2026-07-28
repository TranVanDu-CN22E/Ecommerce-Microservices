using MediatR;

namespace CatalogService.Application.Abstractions.Messaging
{
    public interface IQueryHandler<in TQuery, TResponse>
        : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    { }
}
