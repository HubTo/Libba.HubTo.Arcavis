using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.Endpoint;

public partial class EndpointRepository : Repository<EndpointEntity>, IEndpointRepository
{
    public EndpointRepository(ArcavisContext context, IRequestContext requestContext) : base(context, requestContext)
    { }
}
