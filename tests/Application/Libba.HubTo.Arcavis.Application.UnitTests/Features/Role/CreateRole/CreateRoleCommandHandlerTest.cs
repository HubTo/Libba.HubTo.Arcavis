using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Features.Role.CreateRole;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Role.CreateRole;

public class CreateRoleCommandHandlerTests
{
    #region Mock Dependencies
    private readonly IRoleRepository _roleRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly ILogger<CreateRoleCommandHandler> _loggerMock;
    private readonly CreateRoleCommandHandler _sut;

    public CreateRoleCommandHandlerTests()
    {
        _roleRepositoryMock = Substitute.For<IRoleRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _loggerMock = Substitute.For<ILogger<CreateRoleCommandHandler>>();
        _sut = new CreateRoleCommandHandler(_loggerMock, _roleRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenCalledWithValidCommand_ShouldCreateAndSaveRoleAndReturnId()
    {
        var command = new CreateRoleCommand("TestName", "TestDescription");
        var expectedId = Guid.NewGuid();
        var mappedEntity = new RoleEntity { Id = expectedId };

        _mapperMock.Map<RoleEntity>(command).Returns(mappedEntity);

        var actualId = await _sut.Handle(command, CancellationToken.None);

        await _roleRepositoryMock.Received(1).AddAsync(mappedEntity, Arg.Any<CancellationToken>());

        await _roleRepositoryMock.Received(1).SaveAsync(Arg.Any<CancellationToken>());

        actualId.Should().Be(expectedId);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var command = new CreateRoleCommand("TestName", "TestDescription");
        var mappedEntity = new RoleEntity { Id = Guid.NewGuid() };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mapperMock.Map<RoleEntity>(command).Returns(mappedEntity);

        _roleRepositoryMock
            .AddAsync(mappedEntity, Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Database connection failed");

        await _roleRepositoryMock.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }
}
