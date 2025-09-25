using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Models;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.Role;

public partial class RoleRepository :Repository<RoleEntity>, IRoleRepository
{
    public RoleRepository(ArcavisContext context, IRequestContext requestContext) : base(context, requestContext)
    { }
}
