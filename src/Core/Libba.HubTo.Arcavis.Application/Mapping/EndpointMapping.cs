using Libba.HubTo.Arcavis.Application.Features.Endpoint.GetAllEndpoints;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.GetEndpointById;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.CreateEndpoint;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.UpdateEndpoint;
using Libba.HubTo.Arcavis.Domain.Entities;
using AutoMapper;


namespace Libba.HubTo.Arcavis.Application.Mapping;

public class EndpointMapping : Profile
{
    public EndpointMapping()
    {
        #region Business to Data
        CreateMap<CreateEndpointCommand, EndpointEntity>();
        CreateMap<UpdateEndpointCommand, EndpointEntity>();
        #endregion

        #region Data to Business
        CreateMap<EndpointEntity, EndpointListItemDto>();
        CreateMap<EndpointEntity, EndpointDetailDto>();
        #endregion
    }
}
