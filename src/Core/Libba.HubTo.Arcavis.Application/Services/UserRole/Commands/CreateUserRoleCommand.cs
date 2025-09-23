using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.Commands;

public record CreateUserRoleCommand
(
    Guid UserId,
    Guid RoleId
) : ICommand<Guid>;
