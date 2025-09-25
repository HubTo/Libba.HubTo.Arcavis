using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Features.Role.CreateRole;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<CreateRoleCommandHandler> _logger;
    private readonly IRoleRepository _roleRepository;
    private readonly IArcavisMapper _mapper;


    public CreateRoleCommandHandler(
        ILogger<CreateRoleCommandHandler> logger,
        IRoleRepository roleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Creating Role with Name: {Name}:", 
            nameof(CreateRoleCommand), 
            request.Name);

        try
        {
            var dal = _mapper.Map<RoleEntity>(request);

            await _roleRepository.AddAsync(dal, cancellationToken);
            await _roleRepository.SaveAsync(cancellationToken);

            _logger.LogInformation("Successfully created Role with ID: {RoleId}", dal.Id);

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for Name: {Name}",
                nameof(CreateRoleCommand),
                request.Name);

            throw;
        }
    }
}