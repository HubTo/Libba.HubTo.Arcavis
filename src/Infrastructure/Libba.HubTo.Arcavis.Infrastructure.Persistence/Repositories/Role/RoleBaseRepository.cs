using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Domain.Models;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.Role;

public partial class RoleRepository :Repository<RoleEntity>, IRoleRepository
{
    public RoleRepository(ArcavisContext context) : base(context)
    { }
}
