using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Features.Token.CreateToken;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Token.CreateToken;

public class CreateTokenCommandHandlerTests
{
    #region Mock Dependencies
    private readonly ITokenRepository _tokenRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly CreateTokenCommandHandler _sut;

    public CreateTokenCommandHandlerTests()
    {
        _tokenRepositoryMock = Substitute.For<ITokenRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new CreateTokenCommandHandler(_tokenRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenCalledWithValidCommand_ShouldCreateAndSaveTokenAndReturnId()
    {
        var command = new CreateTokenCommand("TestRefreshToken", DateTime.UtcNow, Guid.NewGuid());
        var expectedId = Guid.NewGuid();
        var mappedEntity = new TokenEntity { Id = expectedId };

        _mapperMock.Map<TokenEntity>(command).Returns(mappedEntity);

        var actualId = await _sut.Handle(command, CancellationToken.None);

        await _tokenRepositoryMock.Received(1).AddAsync(mappedEntity, Arg.Any<CancellationToken>());

        actualId.Should().Be(expectedId);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var command = new CreateTokenCommand("TestRefreshToken", DateTime.UtcNow, Guid.NewGuid());
        var mappedEntity = new TokenEntity { Id = Guid.NewGuid() };
        var expectedException = new InvalidOperationException("Database connection failed");

        _mapperMock.Map<TokenEntity>(command).Returns(mappedEntity);

        _tokenRepositoryMock
            .AddAsync(mappedEntity, Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Database connection failed");
    }
}
