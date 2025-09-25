using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Commands;
using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Dtos;
using Libba.HubTo.Arcavis.Domain.Models;
using AutoMapper;

namespace Libba.HubTo.Arcavis.Application.Mapping;

public class RoleEndpointMapping : Profile
{
    public RoleEndpointMapping()
    {
        #region Business to Data
        CreateMap<CreateRoleEndpointCommand, RoleEndpointEntity>();
        CreateMap<UpdateRoleEndpointCommand, RoleEndpointEntity>();
        #endregion

        #region Data to Business
        CreateMap<RoleEndpointEntity, RoleEndpointDto>();
        #endregion
    }
}
