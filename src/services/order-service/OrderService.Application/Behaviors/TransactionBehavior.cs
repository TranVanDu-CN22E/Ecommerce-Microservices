using MediatR;
using OrderService.Application.Abstractions.Messaging;
using OrderService.Application.Common;
using OrderService.Domain.Interface;

namespace OrderService.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : ICommand<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_unitOfWork.HasActiveTransaction)
                return await next();
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await next();
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return response;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
