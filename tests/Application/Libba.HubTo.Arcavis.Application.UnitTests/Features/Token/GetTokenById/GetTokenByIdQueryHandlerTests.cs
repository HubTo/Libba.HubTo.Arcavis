using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Features.Token.GetTokenById;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Token.GetTokenById;

public class GetTokenByIdQueryHandlerTests
{
    #region Mock Dependencies
    private readonly ITokenRepository _tokenRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly GetTokenByIdQueryHandler _sut;

    public GetTokenByIdQueryHandlerTests()
    {
        _tokenRepositoryMock = Substitute.For<ITokenRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new GetTokenByIdQueryHandler(_tokenRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenTokenWithGivenIdExists_ShouldReturnMappedDto()
    {
        var tokenId = Guid.NewGuid();
        var query = new GetTokenByIdQuery(tokenId);

        var fakeEntity = new TokenEntity { Id = tokenId, RefreshToken = "GetInvoiceById" };

        var fakeDto = new TokenDetailDto { Id = tokenId, RefreshToken = "GetInvoiceById" };

        _tokenRepositoryMock.GetByIdAsync(tokenId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<TokenEntity?>(fakeEntity));

        _mapperMock.Map<TokenDetailDto?>(fakeEntity).Returns(fakeDto);

        var result = await _sut.Handle(query, CancellationToken.None);


        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDto);
        result?.Id.Should().Be(tokenId);
    }

    [Fact]
    public async Task Handle_WhenTokenWithGivenIdDoesNotExist_ShouldReturnNull()
    {
        var nonExistentId = Guid.NewGuid();
        var query = new GetTokenByIdQuery(nonExistentId);

        _tokenRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<TokenEntity?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<TokenDetailDto?>(Arg.Any<TokenEntity>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var tokenId = Guid.NewGuid();
        var query = new GetTokenByIdQuery(tokenId);
        var expectedException = new InvalidOperationException("Database error");

        _tokenRepositoryMock.GetByIdAsync(tokenId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<TokenEntity?>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
