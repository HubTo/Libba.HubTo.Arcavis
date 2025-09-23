using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.Commands;

public record DeleteEndpointCommand
(
    Guid Id
) : ICommand<bool>;