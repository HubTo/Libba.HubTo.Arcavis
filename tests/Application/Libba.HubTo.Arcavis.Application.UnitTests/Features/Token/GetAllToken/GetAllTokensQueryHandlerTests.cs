using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Token;
using Libba.HubTo.Arcavis.Application.Features.Token.GetAllTokens;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Token.GetAllToken;

public class GetAllTokensQueryHandlerTests
{
    #region Mock Dependencies
    private readonly ITokenRepository _tokenRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly GetAllTokensQueryHandler _sut;

    public GetAllTokensQueryHandlerTests()
    {
        _tokenRepositoryMock = Substitute.For<ITokenRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new GetAllTokensQueryHandler(_tokenRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenTokensExist_ShouldReturnMappedDtoList()
    {
        var query = new GetAllTokensQuery();

        var fakeEntityList = new List<TokenEntity>
        {
            new TokenEntity { Id = Guid.NewGuid(), RefreshToken = "RefreshToken1" },
            new TokenEntity { Id = Guid.NewGuid(), RefreshToken = "RefreshToken2" }
        };
        
        var fakeDtoList = new List<TokenListItemDto>
        {
            new TokenListItemDto { Id = fakeEntityList[0].Id, RefreshToken = "RefreshToken1" },
            new TokenListItemDto { Id = fakeEntityList[1].Id, RefreshToken = "RefreshToken2" }
        };

        _tokenRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<TokenEntity>>(fakeEntityList));

        _mapperMock.Map<IEnumerable<TokenListItemDto>>(fakeEntityList).Returns(fakeDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDtoList);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoTokensExist_ShouldReturnNull()
    {
        var query = new GetAllTokensQuery();

        _tokenRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<TokenEntity>?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<IEnumerable<TokenListItemDto>>(Arg.Any<IEnumerable<TokenEntity>>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryReturnsEmptyList_ShouldReturnEmptyDtoList()
    {
        var query = new GetAllTokensQuery();
        var emptyEntityList = new List<TokenEntity>();
        var emptyDtoList = new List<TokenListItemDto>();

        _tokenRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<IEnumerable<TokenEntity>>(emptyEntityList));
        _mapperMock.Map<IEnumerable<TokenListItemDto>>(emptyEntityList).Returns(emptyDtoList);

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var query = new GetAllTokensQuery();
        var expectedException = new InvalidOperationException("Database error");

        _tokenRepositoryMock.GetAllAsync(Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<IEnumerable<TokenEntity>>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
