using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.GetAllEndpoints;

public record GetAllEndpointsQuery() : IQuery<IEnumerable<EndpointListItemDto>>;
