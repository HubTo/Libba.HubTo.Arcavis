using Libba.HubTo.Arcavis.Application.Services.Endpoint.Commands;
using Libba.HubTo.Arcavis.Application.Services.Endpoint.Dtos;
using Libba.HubTo.Arcavis.Domain.Models;
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
        CreateMap<EndpointEntity, EndpointDto>();
        #endregion
    }
}
