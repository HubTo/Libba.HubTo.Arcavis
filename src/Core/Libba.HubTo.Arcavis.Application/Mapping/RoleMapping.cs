using Libba.HubTo.Arcavis.Application.Services.Role.Commands;
using Libba.HubTo.Arcavis.Application.Services.Role.Dtos;
using Libba.HubTo.Arcavis.Domain.Models;
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
        CreateMap<RoleEntity, RoleDto>();
        #endregion
    }
}
