using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.CreateRoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.RoleEndpoint.CreateRoleEndpoint;

public class CreateRoleEndpointCommandHandlerTests
{
    #region Mock Dependencies
    private readonly IRoleEndpointRepository _roleEndpointRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly CreateRoleEndpointCommandHandler _sut;

    public CreateRoleEndpointCommandHandlerTests()
    {
        _roleEndpointRepositoryMock = Substitute.For<IRoleEndpointRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new CreateRoleEndpointCommandHandler(_roleEndpointRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenCalledWithValidCommand_ShouldCreateAndSaveRoleEndpointAndReturnId()
    {
        var fakeRoleId = Guid.NewGuid();
        var fakeEndpointId = Guid.NewGuid();
        var command = new CreateRoleEndpointCommand(fakeRoleId, fakeEndpointId);

        var expectedId = Guid.NewGuid();
        var mappedEntity = new RoleEndpointEntity { Id = expectedId };

        _mapperMock.Map<RoleEndpointEntity>(command).Returns(mappedEntity);

        var actualId = await _sut.Handle(command, CancellationToken.None);

        await _roleEndpointRepositoryMock.Received(1).AddAsync(mappedEntity, Arg.Any<CancellationToken>());

        actualId.Should().Be(expectedId);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var fakeRoleId = Guid.NewGuid();
        var fakeEndpointId = Guid.NewGuid();
        var command = new CreateRoleEndpointCommand(fakeRoleId, fakeEndpointId);
        var mappedEntity = new RoleEndpointEntity { Id = Guid.NewGuid() };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mapperMock.Map<RoleEndpointEntity>(command).Returns(mappedEntity);

        _roleEndpointRepositoryMock
            .AddAsync(mappedEntity, Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Database connection failed");
    }
}
