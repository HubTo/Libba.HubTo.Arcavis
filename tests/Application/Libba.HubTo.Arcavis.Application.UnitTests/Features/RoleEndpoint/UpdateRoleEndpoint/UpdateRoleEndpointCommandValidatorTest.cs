using FluentAssertions;
using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.UpdateRoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.RoleEndpoint.UpdateRoleEndpoint;

public class UpdateRoleEndpointCommandValidatorTests
{
    #region Dependencies
    private readonly IRoleRepository _roleRepositoryMock;
    private readonly IEndpointRepository _endpointRepositoryMock;
    private readonly UpdateRoleEndpointCommandValidator _sut;

    public UpdateRoleEndpointCommandValidatorTests()
    {
        _roleRepositoryMock = Substitute.For<IRoleRepository>();
        _endpointRepositoryMock = Substitute.For<IEndpointRepository>();
        _sut = new UpdateRoleEndpointCommandValidator(_roleRepositoryMock, _endpointRepositoryMock);
    }
    #endregion

    [Fact]
    public async Task GivenValidCommandAndDependencies_WhenValidationIsPerformed_ThenShouldSucceed()
    {
        var command = new UpdateRoleEndpointCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        _roleRepositoryMock.ExistsAsync(command.RoleId, Arg.Any<CancellationToken>()).Returns(true);
        _endpointRepositoryMock.ExistsAsync(command.EndpointId, Arg.Any<CancellationToken>()).Returns(true);
        
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task GivenEmptyId_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new UpdateRoleEndpointCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid());

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Id");
    }

    [Fact]
    public async Task GivenNonExistentRole_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new UpdateRoleEndpointCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        _roleRepositoryMock.ExistsAsync(command.RoleId, Arg.Any<CancellationToken>()).Returns(false);
        _endpointRepositoryMock.ExistsAsync(command.EndpointId, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "RoleId");
    }

    [Fact]
    public async Task GivenNonExistentEndpoint_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new UpdateRoleEndpointCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        _roleRepositoryMock.ExistsAsync(command.RoleId, Arg.Any<CancellationToken>()).Returns(true); 
        _endpointRepositoryMock.ExistsAsync(command.EndpointId, Arg.Any<CancellationToken>()).Returns(false);

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "EndpointId");
    }
}

