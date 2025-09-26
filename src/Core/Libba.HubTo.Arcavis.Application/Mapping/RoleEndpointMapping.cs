using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetRoleEndpointById;
using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetAllRoleEndpoints;
using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.CreateRoleEndpoint;
using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.UpdateRoleEndpoint;
using Libba.HubTo.Arcavis.Domain.Entities;
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
        CreateMap<RoleEndpointEntity, RoleEndpointListItemDto>();
        CreateMap<RoleEndpointEntity, RoleEndpointDetailDto>();
        #endregion
    }
}
