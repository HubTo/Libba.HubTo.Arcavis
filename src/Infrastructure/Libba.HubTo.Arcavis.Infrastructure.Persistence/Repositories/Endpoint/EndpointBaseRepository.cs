using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Domain.Models;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.Endpoint;

public partial class EndpointRepository : Repository<EndpointEntity>, IEndpointRepository
{
    public EndpointRepository(ArcavisContext context) : base(context)
    { }
}
