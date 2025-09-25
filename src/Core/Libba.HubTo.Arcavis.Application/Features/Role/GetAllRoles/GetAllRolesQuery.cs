using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Role.GetAllRoles;

public record GetAllRolesQuery() : IQuery<IEnumerable<RoleListItemDto>>;