using Libba.HubTo.Arcavis.Application.Interfaces;
using MediatR;

namespace Libba.HubTo.Arcavis.Application.Adapters;

public class ArcavisCQRS : IArcavisCQRS
{
    private readonly IMediator _mediator;

    public ArcavisCQRS(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    public async Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(request, cancellationToken);
    }

    public async Task PublishAsync(INotification notification, CancellationToken cancellationToken = default)
    {
        await _mediator.Publish(notification, cancellationToken);
    }
}
