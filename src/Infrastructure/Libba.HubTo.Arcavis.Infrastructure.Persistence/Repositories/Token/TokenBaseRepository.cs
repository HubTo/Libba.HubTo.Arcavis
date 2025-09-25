using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.Token;

public partial class TokenRepository : Repository<TokenEntity>, ITokenRepository
{
    public TokenRepository(ArcavisContext context, IRequestContext requestContext) : base(context, requestContext)
    { }
}
