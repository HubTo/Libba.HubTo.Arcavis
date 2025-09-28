using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;
using MediatR;

namespace Libba.HubTo.Arcavis.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    #region Dependencies
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(
        IUnitOfWork unitOfWork, 
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    #endregion

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        try
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                _logger.LogInformation("--- Begin transaction for {RequestName}", requestName);

                var response = await next();

                _logger.LogInformation("--- Commit transaction for {RequestName}", requestName);

                return response;
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--- Rollback transaction executed for {RequestName}", requestName);
            throw;
        }

    }
}
