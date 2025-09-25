using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.UpdateRoleEndpoint;

public class UpdateRoleEndpointCommandHandler : ICommandHandler<UpdateRoleEndpointCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<UpdateRoleEndpointCommandHandler> _logger;
    private readonly IRoleEndpointRepository _RoleEndpointRepository;
    private readonly IArcavisMapper _mapper;


    public UpdateRoleEndpointCommandHandler(
        ILogger<UpdateRoleEndpointCommandHandler> logger,
        IRoleEndpointRepository RoleEndpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _RoleEndpointRepository = RoleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateRoleEndpointCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Updating RoleEndpoint with RoleId, EndpointId: {RoleId}, {EndpointId}:",
            nameof(UpdateRoleEndpointCommand),
            request.RoleId,
            request.EndpointId);

        try
        {
            var dal = await _RoleEndpointRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                throw new Exception($"RoleEndpoint with Id {request.Id} was not found.");
            }

            _mapper.Map(request, dal);

            _RoleEndpointRepository.Update(dal);
            await _RoleEndpointRepository.SaveAsync(cancellationToken);

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for RoleId, EndpointId: {RoleId}, {EndpointId}:",
                nameof(UpdateRoleEndpointCommand),
                request.RoleId,
                request.EndpointId);

            throw;
        }
    }
}
