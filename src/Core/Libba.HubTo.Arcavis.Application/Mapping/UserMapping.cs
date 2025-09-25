using Libba.HubTo.Arcavis.Application.Features.User.GetUserById;
using Libba.HubTo.Arcavis.Application.Features.User.CreateUser;
using Libba.HubTo.Arcavis.Application.Features.User.GetAllUser;
using Libba.HubTo.Arcavis.Application.Features.User.UpdateUser;
using Libba.HubTo.Arcavis.Domain.Entities;
using AutoMapper;

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
        CreateMap<UserEntity, UserDetailDto>();
        CreateMap<UserEntity, UserListItemDto>();
        #endregion
    }
}
