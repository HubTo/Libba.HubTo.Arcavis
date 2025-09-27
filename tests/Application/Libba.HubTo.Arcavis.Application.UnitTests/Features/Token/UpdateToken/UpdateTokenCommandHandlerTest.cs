using Libba.HubTo.Arcavis.Application.Features.Token.UpdateToken;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Token.UpdateToken;

public class UpdateTokenCommandHandlerTest
{
    #region Mock Dependencies
    private readonly ITokenRepository _tokenRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly UpdateTokenCommandHandler _sut;

    public UpdateTokenCommandHandlerTest()
    {
        _tokenRepositoryMock = Substitute.For<ITokenRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new UpdateTokenCommandHandler(_tokenRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenTokenExist_ShouldUpdateAndReturnId()
    {
        var existingId = Guid.NewGuid();
        var command = new UpdateTokenCommand(existingId, "TestRefreshToken", DateTime.UtcNow, Guid.NewGuid());
        var existingEntity = new TokenEntity { Id = existingId };

        _tokenRepositoryMock.GetByIdAsync(existingId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingEntity));

        var resultId = await _sut.Handle(command, CancellationToken.None);

        _mapperMock.Received(1).Map(command, existingEntity);
        _tokenRepositoryMock.Received(1).Update(existingEntity);

        Assert.Equal(existingId, resultId);
    }

    [Fact]
    public async Task Handle_WhenTokenDoesNotExist_ShouldReturnFalse()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new UpdateTokenCommand(nonExistentId, "TestRefreshToken", DateTime.UtcNow, Guid.NewGuid());

        _tokenRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<TokenEntity?>(null));

        Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
                 .WithMessage($"Token with Id {nonExistentId} was not found.");

        _mapperMock.DidNotReceive().Map(Arg.Any<UpdateTokenCommand>(), Arg.Any<TokenEntity>());
        _tokenRepositoryMock.DidNotReceive().Update(Arg.Any<TokenEntity>());
    }
}
