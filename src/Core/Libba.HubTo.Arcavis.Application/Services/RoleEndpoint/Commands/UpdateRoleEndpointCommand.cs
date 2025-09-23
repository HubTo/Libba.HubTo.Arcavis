using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Commands;

public record UpdateRoleEndpointCommand
(
    Guid Id,
    Guid EndpointId,
    Guid RoleId
) : ICommand<Guid>;
