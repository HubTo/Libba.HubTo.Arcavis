using Libba.HubTo.Arcavis.Application.Interfaces;

namespace Libba.HubTo.Arcavis.Application.Adapters;

public class RequestContext : IRequestContext
{
    public Guid? UserId { get; set; }
}
