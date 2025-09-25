using Libba.HubTo.Arcavis.Application.Services.RoleEndpoint.Dtos;
using Libba.HubTo.Arcavis.Domain.Entities;
using AutoMapper;
using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.CreateRoleEndpoint;
using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.UpdateRoleEndpoint;

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
