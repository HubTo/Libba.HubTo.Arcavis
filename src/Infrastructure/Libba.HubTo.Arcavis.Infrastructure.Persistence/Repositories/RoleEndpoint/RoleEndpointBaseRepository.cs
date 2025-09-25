using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.RoleEndpoint;

public partial class RoleEndpointRepository : Repository<RoleEndpointEntity>, IRoleEndpointRepository
{
    public RoleEndpointRepository(ArcavisContext context, IRequestContext requestContext) : base(context, requestContext)
    { }
}
