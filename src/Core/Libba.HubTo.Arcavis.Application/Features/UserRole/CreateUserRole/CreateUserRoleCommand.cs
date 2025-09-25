using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.CreateUserRole;

public record CreateUserRoleCommand
(
    Guid UserId,
    Guid RoleId
) : ICommand<Guid>;
