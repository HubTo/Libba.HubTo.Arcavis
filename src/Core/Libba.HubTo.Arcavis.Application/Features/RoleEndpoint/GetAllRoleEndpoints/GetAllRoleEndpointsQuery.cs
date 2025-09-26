using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetAllRoleEndpoints;

public record GetAllRoleEndpointsQuery() : IQuery<IEnumerable<RoleEndpointListItemDto>>;