using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.UpdateRoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.RoleEndpoint.UpdateRoleEndpoint;

public class UpdateRoleEndpointCommandHandlerTest
{
    #region Mock Dependencies
    private readonly IRoleEndpointRepository _roleEndpointRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly UpdateRoleEndpointCommandHandler _sut;

    public UpdateRoleEndpointCommandHandlerTest()
    {
        _roleEndpointRepositoryMock = Substitute.For<IRoleEndpointRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new UpdateRoleEndpointCommandHandler(_roleEndpointRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenRoleEndpointExist_ShouldUpdateAndReturnId()
    {
        var existingId = Guid.NewGuid();
        var command = new UpdateRoleEndpointCommand(existingId, Guid.NewGuid(), Guid.NewGuid());
        var existingEntity = new RoleEndpointEntity { Id = existingId };

        _roleEndpointRepositoryMock.GetByIdAsync(existingId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingEntity));

        var resultId = await _sut.Handle(command, CancellationToken.None);

        _mapperMock.Received(1).Map(command, existingEntity);
        _roleEndpointRepositoryMock.Received(1).Update(existingEntity);

        Assert.Equal(existingId, resultId);
    }

    [Fact]
    public async Task Handle_WhenRoleEndpointDoesNotExist_ShouldReturnFalse()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new UpdateRoleEndpointCommand(nonExistentId, Guid.NewGuid(), Guid.NewGuid());

        _roleEndpointRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<RoleEndpointEntity?>(null));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
                 .WithMessage($"RoleEndpoint with Id {nonExistentId} was not found.");

        _mapperMock.DidNotReceive().Map(Arg.Any<UpdateRoleEndpointCommand>(), Arg.Any<RoleEndpointEntity>());
        _roleEndpointRepositoryMock.DidNotReceive().Update(Arg.Any<RoleEndpointEntity>());
    }
}
