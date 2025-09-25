using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.UpdateUserRole;

public record UpdateUserRoleCommand
(
    Guid Id,
    Guid UserId,
    Guid RoleId
) : ICommand<Guid>;