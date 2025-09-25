using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.DeleteRoleEndpoint;

public record DeleteRoleEndpointCommand
(
    Guid Id
) : ICommand<bool>;
