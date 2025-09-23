using Libba.HubTo.Arcavis.Application.Interfaces;
using MediatR;

namespace Libba.HubTo.Arcavis.Application.Services;

public class ArcavisCQRS : IArcavisCQRS
{
    private readonly IMediator _mediator;

    public ArcavisCQRS(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResult> SendAsync<TResult>(IRequest<TResult> request)
    {
        return await _mediator.Send(request);
    }

    public async Task SendAsync(IRequest request)
    {
        await _mediator.Send(request);
    }

    public async Task PublishAsync(INotification notification)
    {
        await _mediator.Publish(notification);
    }
}
