using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.GetAllUsers;

public record GetAllUsersQuery() : IQuery<IEnumerable<UserListItemDto>>;