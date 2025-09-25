using Libba.HubTo.Arcavis.Application.Services.Endpoint.Dtos;
using Libba.HubTo.Arcavis.Domain.Entities;
using AutoMapper;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.CreateEndpoint;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.UpdateEndpoint;

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
        CreateMap<EndpointEntity, EndpointDto>();
        #endregion
    }
}
