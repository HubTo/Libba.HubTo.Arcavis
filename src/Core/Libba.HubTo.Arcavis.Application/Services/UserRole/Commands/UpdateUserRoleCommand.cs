using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.Commands;

public record UpdateUserRoleCommand
(
    Guid Id,
    Guid UserId,
    Guid RoleId
) : ICommand<Guid>;