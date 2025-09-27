using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Features.Token.DeleteToken;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Token.DeleteToken;

public class DeleteTokenCommandHandlerTests
{
    #region Mock Dependencies
    private readonly ITokenRepository _tokenRepositoryMock;
    private readonly DeleteTokenCommandHandler _sut;

    public DeleteTokenCommandHandlerTests()
    {
        _tokenRepositoryMock = Substitute.For<ITokenRepository>();
        _sut = new DeleteTokenCommandHandler(_tokenRepositoryMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenTokenExist_ShouldDeleteAndSaveChanges()
    {
        var tokenId = Guid.NewGuid();
        var command = new DeleteTokenCommand(tokenId);
        var fakeTokenEntity = new TokenEntity { Id = tokenId };

        _tokenRepositoryMock.GetByIdAsync(tokenId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(fakeTokenEntity));

        await _sut.Handle(command, CancellationToken.None);

        _tokenRepositoryMock.Received(1).Delete(fakeTokenEntity);
    }

    [Fact]
    public async Task Handle_WhenTokenDoesNotExist_ShouldThrowNotFoundException()
    {
        var nonExistentId = Guid.NewGuid();
        var command = new DeleteTokenCommand(nonExistentId);

        _tokenRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<TokenEntity?>(null));

        var result = await _sut.Handle(command, CancellationToken.None);

        result.Should().BeFalse();

        _tokenRepositoryMock.DidNotReceive().Delete(Arg.Any<TokenEntity>());
    }
}
