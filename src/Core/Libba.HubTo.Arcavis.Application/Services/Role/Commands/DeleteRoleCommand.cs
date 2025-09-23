using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.Role.Commands;

public record DeleteRoleCommand
(
    Guid Id
) : ICommand<bool>;
