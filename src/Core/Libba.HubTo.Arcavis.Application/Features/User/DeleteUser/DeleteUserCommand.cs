using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.DeleteUser;

public record DeleteUserCommand
(
    Guid Id
) : ICommand<bool>;
