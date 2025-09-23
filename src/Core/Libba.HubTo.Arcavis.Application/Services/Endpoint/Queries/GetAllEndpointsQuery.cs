using Libba.HubTo.Arcavis.Application.Services.Endpoint.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.Queries;

public record GetAllEndpointsQuery() : IQuery<List<EndpointDto>>;
