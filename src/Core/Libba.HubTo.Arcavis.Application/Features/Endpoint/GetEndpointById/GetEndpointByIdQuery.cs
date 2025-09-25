using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.GetEndpointById;

public record GetEndpointByIdQuery
(
    Guid Id
) : IQuery<EndpointDetailDto?>;

