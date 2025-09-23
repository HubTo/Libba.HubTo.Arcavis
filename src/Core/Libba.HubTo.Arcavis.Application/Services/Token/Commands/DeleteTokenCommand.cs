using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.Token.Commands;

public record DeleteTokenCommand
(
    Guid Id
) : ICommand<bool>;
