using Libba.HubTo.Arcavis.Domain.Enums;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.Dtos;

public class EndpointDto
{
    public Guid Id { get; set; }
    public string ModuleName { get; set; }
    public string ControllerName { get; set; }
    public string ActionName { get; set; }
    public HttpVerb HttpVerb { get; set; }
    public string Namespace { get; set; }
}