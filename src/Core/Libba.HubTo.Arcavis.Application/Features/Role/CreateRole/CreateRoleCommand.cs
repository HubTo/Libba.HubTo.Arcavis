using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Role.CreateRole;

public record CreateRoleCommand
(
    string Name,
    string? Description
) : ICommand<Guid>;
