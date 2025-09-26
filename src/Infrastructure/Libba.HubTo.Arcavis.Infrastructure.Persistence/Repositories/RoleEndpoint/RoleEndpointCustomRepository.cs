using Microsoft.EntityFrameworkCore;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.RoleEndpoint;

public partial class RoleEndpointRepository
{
    public async Task<bool> DoesRelationExistAsync(Guid roleId, Guid endpointId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(re => re.RoleId == roleId && re.EndpointId == endpointId, cancellationToken);
    }
}
