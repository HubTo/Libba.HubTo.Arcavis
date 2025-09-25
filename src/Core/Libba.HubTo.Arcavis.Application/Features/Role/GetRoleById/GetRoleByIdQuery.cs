using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.Role.GetRoleById;

public record GetRoleByIdQuery
(
    Guid Id
) : IQuery<RoleDetailDto?>;