using Microsoft.EntityFrameworkCore;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.UserRole;

public partial class UserRoleRepository
{
    public async Task<bool> DoesRelationExistAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(re => re.UserId == userId && re.RoleId == roleId, cancellationToken);
    }
}
