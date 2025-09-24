using Libba.HubTo.Arcavis.Application.Interfaces;

namespace Libba.HubTo.Arcavis.Application.Services;

public class RequestContext : IRequestContext
{
    public Guid? UserId { get; set; }
}
