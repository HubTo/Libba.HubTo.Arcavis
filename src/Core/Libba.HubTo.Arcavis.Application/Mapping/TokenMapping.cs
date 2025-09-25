using Libba.HubTo.Arcavis.Application.Features.Token.GetTokenById;
using Libba.HubTo.Arcavis.Application.Features.Token.CreateToken;
using Libba.HubTo.Arcavis.Application.Features.Token.UpdateToken;
using Libba.HubTo.Arcavis.Application.Features.Token.GetAllToken;
using Libba.HubTo.Arcavis.Domain.Entities;
using AutoMapper;

namespace Libba.HubTo.Arcavis.Application.Mapping;

public class TokenMapping : Profile
{
    public TokenMapping()
    {
        #region Business to Data
        CreateMap<CreateTokenCommand, TokenEntity>();
        CreateMap<UpdateTokenCommand, TokenEntity>();
        #endregion

        #region Data to Business
        CreateMap<TokenEntity, TokenListItemDto>();
        CreateMap<TokenEntity, TokenDetailDto>();
        #endregion
    }
}
