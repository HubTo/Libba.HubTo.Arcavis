using Libba.HubTo.Arcavis.Application.Services.UserRole.Dtos;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Services.UserRole.Queries;

public record GetUserRoleByIdQuery
(
    Guid Id
) : IQuery<UserRoleDto?>;