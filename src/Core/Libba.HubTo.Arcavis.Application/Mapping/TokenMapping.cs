using Libba.HubTo.Arcavis.Application.Services.Token.Commands;
using Libba.HubTo.Arcavis.Application.Services.Token.Dtos;
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
        CreateMap<TokenEntity, TokenDto>();
        #endregion
    }
}
