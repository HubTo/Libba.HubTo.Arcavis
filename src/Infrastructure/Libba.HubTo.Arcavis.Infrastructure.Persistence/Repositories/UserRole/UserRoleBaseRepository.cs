using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Domain.Models;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.UserRole;

public partial class UserRoleRepository : Repository<UserRoleEntity>, IUserRoleRepository
{
    public UserRoleRepository(ArcavisContext context) : base(context)
    { }
}
