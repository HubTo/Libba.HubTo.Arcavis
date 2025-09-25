using Libba.HubTo.Arcavis.Application.Features.Role.GetRoleById;
using Libba.HubTo.Arcavis.Application.Features.Role.GetAllRoles;
using Libba.HubTo.Arcavis.Application.Features.Role.CreateRole;
using Libba.HubTo.Arcavis.Application.Features.Role.UpdateRole;
using Libba.HubTo.Arcavis.Domain.Entities;
using AutoMapper;

namespace Libba.HubTo.Arcavis.Application.Mapping;

public class RoleMapping : Profile
{
    public RoleMapping()
    {
        #region Business to Data
        CreateMap<CreateRoleCommand, RoleEntity>();
        CreateMap<UpdateRoleCommand, RoleEntity>();
        #endregion

        #region Data to Business
        CreateMap<RoleEntity, RoleListItemDto>();
        CreateMap<RoleEntity, RoleDetailDto>();
        #endregion
    }
}
