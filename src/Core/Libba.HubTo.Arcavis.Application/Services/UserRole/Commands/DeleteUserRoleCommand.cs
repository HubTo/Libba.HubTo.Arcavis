using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.Commands;

public record DeleteUserRoleCommand
(
    Guid Id
) : ICommand<bool>;
