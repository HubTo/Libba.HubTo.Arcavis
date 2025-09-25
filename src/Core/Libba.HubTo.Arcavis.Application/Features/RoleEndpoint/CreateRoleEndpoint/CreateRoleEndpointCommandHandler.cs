using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.CreateRoleEndpoint;

public class CreateRoleEndpointCommandHandler : ICommandHandler<CreateRoleEndpointCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<CreateRoleEndpointCommandHandler> _logger;
    private readonly IRoleEndpointRepository _roleEndpointRepository;
    private readonly IArcavisMapper _mapper;


    public CreateRoleEndpointCommandHandler(
        ILogger<CreateRoleEndpointCommandHandler> logger,
        IRoleEndpointRepository roleEndpointRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleEndpointRepository = roleEndpointRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(CreateRoleEndpointCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Creating RoleEndpoint with RoleId, EndpointId: {RoleId}, {EndpointId}:", 
            nameof(CreateRoleEndpointCommand), 
            request.RoleId,
            request.EndpointId);

        try
        {
            var dal = _mapper.Map<RoleEndpointEntity>(request);

            await _roleEndpointRepository.AddAsync(dal, cancellationToken);
            await _roleEndpointRepository.SaveAsync(cancellationToken);

            _logger.LogInformation("Successfully created RoleEndpoint with ID: {RoleEndpointId}", dal.Id);

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for RoleId, EndpointId: {RoleId}, {EndpointId}:",
                nameof(CreateRoleEndpointCommand),
                request.RoleId,
                request.EndpointId);

            throw;
        }
    }
}