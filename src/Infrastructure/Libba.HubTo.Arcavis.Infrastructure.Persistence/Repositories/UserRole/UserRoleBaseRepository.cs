using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.UserRole;

public partial class UserRoleRepository : Repository<UserRoleEntity>, IUserRoleRepository
{
    public UserRoleRepository(ArcavisContext context, IRequestContext requestContext) : base(context, requestContext)
    { }
}
