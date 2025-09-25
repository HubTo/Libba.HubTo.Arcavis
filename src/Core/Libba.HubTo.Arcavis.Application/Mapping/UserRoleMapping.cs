using Libba.HubTo.Arcavis.Application.Services.UserRole.Commands;
using Libba.HubTo.Arcavis.Application.Services.UserRole.Dtos;
using Libba.HubTo.Arcavis.Domain.Models;
using AutoMapper;

namespace Libba.HubTo.Arcavis.Application.Mapping;

public class UserRoleMapping : Profile
{
    public UserRoleMapping()
    {
        #region Business to Data
        CreateMap<CreateUserRoleCommand, UserRoleEntity>();
        CreateMap<UpdateUserRoleCommand, UserRoleEntity>();
        #endregion

        #region Data to Business
        CreateMap<UserRoleEntity, UserRoleDto>();
        #endregion
    }
}
