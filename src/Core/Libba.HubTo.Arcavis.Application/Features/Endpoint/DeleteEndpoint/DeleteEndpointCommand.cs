using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.DeleteEndpoint;

public record DeleteEndpointCommand
(
    Guid Id
) : ICommand<bool>;