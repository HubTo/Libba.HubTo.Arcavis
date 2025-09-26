namespace Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;

public partial interface IRoleEndpointRepository
{
    Task<bool> DoesRelationExistAsync(Guid roleId, Guid endpointId, CancellationToken cancellationToken = default);
}
