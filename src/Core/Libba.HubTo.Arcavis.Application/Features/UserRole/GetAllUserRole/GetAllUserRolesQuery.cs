using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.GetAllUserRole;

public record GetAllUserRolesQuery() : IQuery<IEnumerable<UserRoleListItemDto>>;