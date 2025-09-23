using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.User.Commands;

public record DeleteUserCommand
(
    Guid Id
) : ICommand<bool>;
