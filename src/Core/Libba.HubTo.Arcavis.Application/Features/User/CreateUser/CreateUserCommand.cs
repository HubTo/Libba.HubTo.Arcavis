using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.CreateUser;

public record CreateUserCommand
(
    string PhoneCode,
    string PhoneNumber
) : ICommand<Guid>;
