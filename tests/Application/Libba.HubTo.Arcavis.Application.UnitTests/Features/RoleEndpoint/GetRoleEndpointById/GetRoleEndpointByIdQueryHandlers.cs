using Libba.HubTo.Arcavis.Application.Features.RoleEndpoint.GetRoleEndpointById;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.RoleEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.RoleEndpoint.GetRoleEndpointById;

public class GetRoleEndpointByIdQueryHandlerTests
{
    #region Mock Dependencies
    private readonly IRoleEndpointRepository _roleEndpointRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly GetRoleEndpointByIdQueryHandler _sut;

    public GetRoleEndpointByIdQueryHandlerTests()
    {
        _roleEndpointRepositoryMock = Substitute.For<IRoleEndpointRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new GetRoleEndpointByIdQueryHandler(_roleEndpointRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenRoleEndpointWithGivenIdExists_ShouldReturnMappedDto()
    {
        var roleEndpointId = Guid.NewGuid();
        var fakeRoleId = Guid.NewGuid();
        var query = new GetRoleEndpointByIdQuery(roleEndpointId);

        var fakeEntity = new RoleEndpointEntity { Id = roleEndpointId, RoleId = fakeRoleId };
        var fakeDto = new RoleEndpointDetailDto { Id = roleEndpointId, RoleId = fakeRoleId };

        _roleEndpointRepositoryMock.GetByIdAsync(roleEndpointId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<RoleEndpointEntity?>(fakeEntity));

        _mapperMock.Map<RoleEndpointDetailDto?>(fakeEntity).Returns(fakeDto);

        var result = await _sut.Handle(query, CancellationToken.None);


        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDto);
        result?.Id.Should().Be(roleEndpointId);
    }

    [Fact]
    public async Task Handle_WhenRoleEndpointWithGivenIdDoesNotExist_ShouldReturnNull()
    {
        var nonExistentId = Guid.NewGuid();
        var query = new GetRoleEndpointByIdQuery(nonExistentId);

        _roleEndpointRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<RoleEndpointEntity?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<RoleEndpointDetailDto?>(Arg.Any<RoleEndpointEntity>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var roleEndpointId = Guid.NewGuid();
        var query = new GetRoleEndpointByIdQuery(roleEndpointId);
        var expectedException = new InvalidOperationException("Database error");

        _roleEndpointRepositoryMock.GetByIdAsync(roleEndpointId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<RoleEndpointEntity?>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
