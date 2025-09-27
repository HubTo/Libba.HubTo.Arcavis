using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Features.UserRole.GetUserRoleById;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.UserRole.GetUserRoleById;

public class GetUserRoleByIdQueryHandlerTests
{
    #region Mock Dependencies
    private readonly IUserRoleRepository _userRoleRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly GetUserRoleByIdQueryHandler _sut;

    public GetUserRoleByIdQueryHandlerTests()
    {
        _userRoleRepositoryMock = Substitute.For<IUserRoleRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new GetUserRoleByIdQueryHandler(_userRoleRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenUserRoleWithGivenIdExists_ShouldReturnMappedDto()
    {
        var userRoleId = Guid.NewGuid();
        var query = new GetUserRoleByIdQuery(userRoleId);

        var fakeEntity = new UserRoleEntity { Id = userRoleId, UserId = Guid.NewGuid() };

        var fakeDto = new UserRoleDetailDto { Id = userRoleId, UserId = Guid.NewGuid() };

        _userRoleRepositoryMock.GetByIdAsync(userRoleId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<UserRoleEntity?>(fakeEntity));

        _mapperMock.Map<UserRoleDetailDto?>(fakeEntity).Returns(fakeDto);

        var result = await _sut.Handle(query, CancellationToken.None);


        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDto);
        result?.Id.Should().Be(userRoleId);
    }

    [Fact]
    public async Task Handle_WhenUserRoleWithGivenIdDoesNotExist_ShouldReturnNull()
    {
        var nonExistentId = Guid.NewGuid();
        var query = new GetUserRoleByIdQuery(nonExistentId);

        _userRoleRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<UserRoleEntity?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<UserRoleDetailDto?>(Arg.Any<UserRoleEntity>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var userRoleId = Guid.NewGuid();
        var query = new GetUserRoleByIdQuery(userRoleId);
        var expectedException = new InvalidOperationException("Database error");

        _userRoleRepositoryMock.GetByIdAsync(userRoleId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<UserRoleEntity?>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
