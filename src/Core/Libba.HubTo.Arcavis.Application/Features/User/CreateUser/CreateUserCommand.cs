using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.User.CreateUser;

public record CreateUserCommand
(
    string Email,
    string PhoneCode,
    string PhoneNumber,
    string Password
) : ICommand<Guid>;
