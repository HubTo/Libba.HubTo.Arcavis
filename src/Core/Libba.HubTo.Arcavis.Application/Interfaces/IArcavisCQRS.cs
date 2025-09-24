using MediatR;

namespace Libba.HubTo.Arcavis.Application.Interfaces;

public interface IArcavisCQRS
{
    Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default);
    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);
    Task PublishAsync(INotification notification, CancellationToken cancellationToken = default);
}
