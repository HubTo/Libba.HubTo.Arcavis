using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.CreateRoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.RoleEndpoint.CreateRole;

public class CreateRoleEndpointCommandValidatorTests
{
    private readonly IRoleRepository _roleRepositoryMock;
    private readonly IEndpointRepository _endpointRepositoryMock;
    private readonly IRoleEndpointRepository _roleEndpointRepositoryMock;
    private readonly CreateRoleEndpointCommandValidator _sut;

    public CreateRoleEndpointCommandValidatorTests()
    {
        _roleRepositoryMock = Substitute.For<IRoleRepository>();
        _endpointRepositoryMock = Substitute.For<IEndpointRepository>();
        _roleEndpointRepositoryMock = Substitute.For<IRoleEndpointRepository>();

        _sut = new CreateRoleEndpointCommandValidator(_roleRepositoryMock, _endpointRepositoryMock, _roleEndpointRepositoryMock);
    }

    [Fact]
    public async Task GivenValidCommandAndDependencies_WhenValidationIsPerformed_ThenShouldSucceed()
    {
        var command = new CreateRoleEndpointCommand(Guid.NewGuid(), Guid.NewGuid());

        _roleRepositoryMock.ExistsAsync(command.RoleId, Arg.Any<CancellationToken>()).Returns(true);
        _endpointRepositoryMock.ExistsAsync(command.EndpointId, Arg.Any<CancellationToken>()).Returns(true);
        _roleEndpointRepositoryMock.DoesRelationExistAsync(command.RoleId, command.EndpointId, Arg.Any<CancellationToken>()).Returns(false);
        
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task GivenEmptyRoleId_WhenValidationIsPerformed_ThenShouldFailForRoleId()
    {
        var command = new CreateRoleEndpointCommand(
            RoleId: Guid.Empty,
            EndpointId: Guid.NewGuid()
        );

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle(e => e.PropertyName == "RoleId");
    }

    [Fact]
    public async Task GivenEmptyEndpointId_WhenValidationIsPerformed_ThenShouldFailForEndpointId()
    {
        var command = new CreateRoleEndpointCommand(
            RoleId: Guid.NewGuid(),
            EndpointId: Guid.Empty
        );

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle(e => e.PropertyName == "EndpointId");
    }

    [Fact]
    public async Task GivenNonExistentRole_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateRoleEndpointCommand(Guid.NewGuid(), Guid.NewGuid());

        _roleRepositoryMock.ExistsAsync(command.RoleId, Arg.Any<CancellationToken>()).Returns(false);
        _endpointRepositoryMock.ExistsAsync(command.EndpointId, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "RoleId" && e.ErrorMessage.Contains("does not exist"));
    }

    [Fact]
    public async Task GivenNonExistentEndpoint_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateRoleEndpointCommand(Guid.NewGuid(), Guid.NewGuid());

        _roleRepositoryMock.ExistsAsync(command.RoleId, Arg.Any<CancellationToken>()).Returns(true);
        _endpointRepositoryMock.ExistsAsync(command.EndpointId, Arg.Any<CancellationToken>()).Returns(false);

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "EndpointId" && e.ErrorMessage.Contains("does not exist"));
    }

    [Fact]
    public async Task GivenExistingRelation_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateRoleEndpointCommand(Guid.NewGuid(), Guid.NewGuid());

        _roleRepositoryMock.ExistsAsync(command.RoleId, Arg.Any<CancellationToken>()).Returns(true);
        _endpointRepositoryMock.ExistsAsync(command.EndpointId, Arg.Any<CancellationToken>()).Returns(true);
        _roleEndpointRepositoryMock.DoesRelationExistAsync(command.RoleId, command.EndpointId, Arg.Any<CancellationToken>()).Returns(true); // İlişki ZATEN VAR

        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => string.IsNullOrEmpty(e.PropertyName) && e.ErrorMessage.Contains("already exists"));
    }
}
