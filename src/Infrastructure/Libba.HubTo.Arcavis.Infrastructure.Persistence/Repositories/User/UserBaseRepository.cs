using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;
using Libba.HubTo.Arcavis.Domain.Models;

namespace Libba.HubTo.Arcavis.Infrastructure.Persistence.Repositories.User;

public partial class UserRepository : Repository<UserEntity>, IUserRepository
{
    public UserRepository(ArcavisContext context) : base(context)
    { }
}
