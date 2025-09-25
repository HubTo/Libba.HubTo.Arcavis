using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Enums;

namespace Libba.HubTo.Arcavis.Application.Features.Endpoint.CreateEndpoint;

public record CreateEndpointCommand
(
    string ModuleName,
    string ControllerName,
    string ActionName,
    HttpVerb HttpVerb,
    string Namespace
) : ICommand<Guid>;
