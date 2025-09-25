using Libba.HubTo.Arcavis.Application.Services.Role.Dtos;
using Libba.HubTo.Arcavis.Domain.Entities;
using AutoMapper;
using Libba.HubTo.Arcavis.Application.Features.Role.CreateRole;
using Libba.HubTo.Arcavis.Application.Features.Role.UpdateRole;

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
        CreateMap<RoleEntity, RoleDto>();
        #endregion
    }
}
