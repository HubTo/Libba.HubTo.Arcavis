using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.GetAllUserRoles;

public record GetAllUserRolesQuery() : IQuery<IEnumerable<UserRoleListItemDto>>;