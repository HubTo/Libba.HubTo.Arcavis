using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Queries;

public record GetAllRoleEndpointsQuery() : IQuery<List<RoleEndpointDto>>;