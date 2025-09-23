using MediatR;

namespace Libba.HubTo.Arcavis.Application.Interfaces;

public interface IArcavisCQRS
{
    Task<TResult> SendAsync<TResult>(IRequest<TResult> request);
    Task SendAsync(IRequest request);
    Task PublishAsync(INotification notification);
}
