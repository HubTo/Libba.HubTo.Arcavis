using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Role.UpdateRole;

public record UpdateRoleCommand
(
    Guid Id,
    string Name,
    string? Description
) : ICommand<Guid>;
