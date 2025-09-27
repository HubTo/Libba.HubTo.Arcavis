using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Token.GetAllTokens;

public record GetAllTokensQuery() : IQuery<IEnumerable<TokenListItemDto>>;