using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Enums;

namespace Libba.HubTo.Arcavis.Application.Services.Endpoint.Commands;

public record CreateEndpointCommand
(
    string ModuleName,
    string ControllerName,
    string ActionName,
    HttpVerb HttpVerb,
    string NameSpace
) : ICommand<Guid>;
