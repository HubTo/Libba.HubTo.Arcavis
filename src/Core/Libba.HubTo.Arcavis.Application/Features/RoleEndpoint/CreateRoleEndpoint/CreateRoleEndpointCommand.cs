using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.CreateRoleEndpoint;

public record CreateRoleEndpointCommand
(
    Guid EndpointId,
    Guid RoleId
) : ICommand<Guid>;
