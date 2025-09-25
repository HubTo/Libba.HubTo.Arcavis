using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.DeleteUserRole;

public record DeleteUserRoleCommand
(
    Guid Id
) : ICommand<bool>;
