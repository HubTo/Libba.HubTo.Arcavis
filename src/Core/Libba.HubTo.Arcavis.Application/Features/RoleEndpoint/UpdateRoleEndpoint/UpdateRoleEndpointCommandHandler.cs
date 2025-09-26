using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.UpdateRoleEndpoint;

public class UpdateRoleEndpointCommandHandler : ICommandHandler<UpdateRoleEndpointCommand, Guid>
{
    #region Dependencies
    private readonly IRoleEndpointRepository _RoleEndpointRepository;
    private readonly IArcavisMapper _mapper;

    public UpdateRoleEndpointCommandHandler(
        IRoleEndpointRepository RoleEndpointRepository,
        IArcavisMapper mapper)
    {
        _RoleEndpointRepository = RoleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateRoleEndpointCommand request, CancellationToken cancellationToken)
    {
        var dal = await _RoleEndpointRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dal is null)
            throw new Exception($"RoleEndpoint with Id {request.Id} was not found.");

        _mapper.Map(request, dal);

        _RoleEndpointRepository.Update(dal);

        return dal.Id;
    }
}
