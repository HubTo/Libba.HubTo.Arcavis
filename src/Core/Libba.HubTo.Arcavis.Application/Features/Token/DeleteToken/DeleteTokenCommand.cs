using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Token.DeleteToken;

public record DeleteTokenCommand
(
    Guid Id
) : ICommand<bool>;
