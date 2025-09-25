using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Role.DeleteRole;

public record DeleteRoleCommand
(
    Guid Id
) : ICommand<bool>;
