using Libba.HubTo.Arcavis.Application.Models.Token;

namespace Libba.HubTo.Arcavis.Application.Interfaces;

public interface ITokenService
{
    Task<SignInModel> GenerateTokensAsync(Guid sessionToken);
}
