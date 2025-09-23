using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;
using Libba.HubTo.Arcavis.Domain.Models;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.UserRole;

public partial class UserRoleRepository : Repository<UserRoleEntity>, IUserRoleRepository
{
    public UserRoleRepository(ArcavisContext context) : base(context)
    { }
}
