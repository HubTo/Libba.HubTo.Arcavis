using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.Role.Commands;

public record UpdateRoleCommand
(
    Guid Id,
    string Name,
    string? Description
) : ICommand<Guid>;
