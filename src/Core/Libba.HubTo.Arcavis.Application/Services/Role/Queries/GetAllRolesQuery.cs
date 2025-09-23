using Libba.HubTo.Arcavis.Application.Services.Role.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.Role.Queries;

public record GetAllRolesQuery() : IQuery<List<RoleDto>>;