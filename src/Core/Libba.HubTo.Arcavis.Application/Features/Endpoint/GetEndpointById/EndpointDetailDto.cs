using Libba.HubTo.Arcavis.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.GetEndpointById;

public class EndpointDetailDto
{
    public Guid Id { get; set; }
    public string ModuleName { get; set; }
    public string ControllerName { get; set; }
    public string ActionName { get; set; }
    public HttpVerb HttpVerb { get; set; }
    public string Namespace { get; set; }
}