using Libba.HubTo.Arcavis.Application.Services.User.Dtos;
using Libba.HubTo.Arcavis.Domain.Entities;
using AutoMapper;
using Libba.HubTo.Arcavis.Application.Features.User.CreateUser;
using Libba.HubTo.Arcavis.Application.Features.User.UpdateUser;

namespace Libba.HubTo.Arcavis.Application.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        #region Business to Data
        CreateMap<CreateUserCommand, UserEntity>();
        CreateMap<UpdateUserCommand, UserEntity>();
        #endregion

        #region Data to Business
        CreateMap<UserEntity, UserDto>();
        #endregion
    }
}
