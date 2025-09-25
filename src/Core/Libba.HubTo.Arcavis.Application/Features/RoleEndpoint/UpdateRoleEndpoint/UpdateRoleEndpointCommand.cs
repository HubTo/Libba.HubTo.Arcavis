using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.UpdateRoleEndpoint;

public record UpdateRoleEndpointCommand
(
    Guid Id,
    Guid EndpointId,
    Guid RoleId
) : ICommand<Guid>;
