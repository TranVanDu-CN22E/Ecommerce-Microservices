using MediatR;

namespace OrderService.Application.Abstractions.Messaging
{
    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }
}
