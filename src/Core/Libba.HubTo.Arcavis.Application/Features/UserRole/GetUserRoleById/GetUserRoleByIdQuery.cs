using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.UserRole.GetUserRoleById;

public record GetUserRoleByIdQuery
(
    Guid Id
) : IQuery<UserRoleDetailDto?>;