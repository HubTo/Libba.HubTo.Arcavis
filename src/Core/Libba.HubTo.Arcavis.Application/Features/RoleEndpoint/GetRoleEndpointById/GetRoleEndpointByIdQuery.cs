using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetRoleEndpointById;

public record GetRoleEndpointByIdQuery
(
    Guid Id
) : IQuery<RoleEndpointDetailDto?>;