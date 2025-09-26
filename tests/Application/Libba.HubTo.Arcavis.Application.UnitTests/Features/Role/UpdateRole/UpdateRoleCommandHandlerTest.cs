using Libba.HubTo.Arcavis.Application.Features.Role.UpdateRole;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Role.UpdateRole;

public class UpdateRoleCommandHandlerTest
{
    #region Mock Dependencies
    private readonly IRoleRepository _roleRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly UpdateRoleCommandHandler _sut;

    public UpdateRoleCommandHandlerTest()
    {
        _roleRepositoryMock = Substitute.For<IRoleRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new UpdateRoleCommandHandler(_roleRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenRoleExist_ShouldUpdateAndReturnId()
    {
        var existingId = Guid.NewGuid();
        var command = new UpdateRoleCommand(existingId, "TestName", "TestDescription");
        var existingEntity = new RoleEntity { Id = existingId };

        _roleRepositoryMock.GetByIdAsync(existingId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingEntity));

        var resultId = await _sut.Handle(command, CancellationToken.None);

        _mapperMock.Received(1).Map(command, existingEntity);
        _roleRepositoryMock.Received(1).Update(existingEntity);

        Assert.Equal(existingId, resultId);
    }

    [Fact]
    public async Task Handle_WhenRoleDoesNotExist_ShouldReturnFalse()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new UpdateRoleCommand(nonExistentId, "TestName", "TestDescription");

        _roleRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<RoleEntity?>(null));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
                 .WithMessage($"Role with Id {nonExistentId} was not found.");

        _mapperMock.DidNotReceive().Map(Arg.Any<UpdateRoleCommand>(), Arg.Any<RoleEntity>());
        _roleRepositoryMock.DidNotReceive().Update(Arg.Any<RoleEntity>());
    }
}
