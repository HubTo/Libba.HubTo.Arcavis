using Libba.HubTo.Arcavis.Application.Features.UserRole.GetUserRoleById;
using Libba.HubTo.Arcavis.Application.Features.UserRole.UpdateUserRole;
using Libba.HubTo.Arcavis.Application.Features.UserRole.CreateUserRole;
using Libba.HubTo.Arcavis.Application.Features.UserRole.GetAllUserRoles;
using Libba.HubTo.Arcavis.Domain.Entities;
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
        CreateMap<UserRoleEntity, UserRoleDetailDto>();
        CreateMap<UserRoleEntity, UserRoleListItemDto>();
        #endregion
    }
}
