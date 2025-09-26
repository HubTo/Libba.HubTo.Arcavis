using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;
using MediatR;

namespace Libba.HubTo.Arcavis.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(IUnitOfWork unitOfWork, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        try
        {
            if (_unitOfWork.HasActiveTransaction)
            {
                return await next();
            }

            TResponse response = default!;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            _logger.LogInformation("--- Begin transaction for {RequestName}", requestName);

            response = await next();

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            _logger.LogInformation("--- Commit transaction for {RequestName}", requestName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--- Rollback transaction executed for {RequestName}", requestName);

            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            throw;
        }
    }
}
