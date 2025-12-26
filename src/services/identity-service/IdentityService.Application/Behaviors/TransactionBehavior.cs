using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Domain.Interfaces;
using MediatR;

namespace IdentityService.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (_unitOfWork.HasActiveTransaction)
            return await next();

        await _unitOfWork.BeginTransactionAsync(ct);

        try
        {
            var response = await next();
            await _unitOfWork.CommitTransactionAsync(ct);
            return response;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}