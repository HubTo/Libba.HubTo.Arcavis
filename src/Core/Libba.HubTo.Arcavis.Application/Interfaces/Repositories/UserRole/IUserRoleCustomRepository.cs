namespace Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;

public partial interface IUserRoleRepository
{
    Task<bool> DoesRelationExistAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
}
