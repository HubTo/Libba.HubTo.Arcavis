using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Services.Role.Commands;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Application.CQRS;
using Microsoft.Extensions.Logging;

namespace Libba.HubTo.Arcavis.Application.Services.Role.RequestHandlers.Commands;

public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, Guid>
{
    #region Dependencies
    private readonly ILogger<UpdateRoleCommandHandler> _logger;
    private readonly IRoleRepository _roleRepository;
    private readonly IArcavisMapper _mapper;


    public UpdateRoleCommandHandler(
        ILogger<UpdateRoleCommandHandler> logger,
        IRoleRepository roleRepository,
        IArcavisMapper mapper)
    {
        _logger = logger;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    #endregion

    public async Task<Guid> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {CommandName}: Updating Role with Name: {Name}:",
            nameof(UpdateRoleCommand),
            request.Name);

        try
        {
            var dal = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (dal is null)
            {
                throw new Exception($"Role with Id {request.Id} was not found.");
            }

            _mapper.Map(request, dal);

            _roleRepository.Update(dal);
            await _roleRepository.SaveAsync();

            return dal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling {CommandName} for Name: {Name}",
                nameof(UpdateRoleCommand),
                request.Name);

            throw;
        } 
    }
}
