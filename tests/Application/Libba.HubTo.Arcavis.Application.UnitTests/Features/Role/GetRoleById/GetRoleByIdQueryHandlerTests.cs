using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Features.Role.GetRoleById;
using Libba.HubTo.Arcavis.Application.Interfaces;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Role.GetRoleById;

public class GetRoleByIdQueryHandlerTests
{
    #region Mock Dependencies
    private readonly IRoleRepository _roleRepositoryMock;
    private readonly IArcavisMapper _mapperMock;
    private readonly GetRoleByIdQueryHandler _sut;

    public GetRoleByIdQueryHandlerTests()
    {
        _roleRepositoryMock = Substitute.For<IRoleRepository>();
        _mapperMock = Substitute.For<IArcavisMapper>();
        _sut = new GetRoleByIdQueryHandler(_roleRepositoryMock, _mapperMock);
    }
    #endregion

    [Fact]
    public async Task Handle_WhenRoleWithGivenIdExists_ShouldReturnMappedDto()
    {
        var roleId = Guid.NewGuid();
        var query = new GetRoleByIdQuery(roleId);

        var fakeEntity = new RoleEntity { Id = roleId, Name = "GetInvoiceById" };

        var fakeDto = new RoleDetailDto { Id = roleId, Name = "GetInvoiceById" };

        _roleRepositoryMock.GetByIdAsync(roleId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<RoleEntity?>(fakeEntity));

        _mapperMock.Map<RoleDetailDto?>(fakeEntity).Returns(fakeDto);

        var result = await _sut.Handle(query, CancellationToken.None);


        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(fakeDto);
        result?.Id.Should().Be(roleId);
    }

    [Fact]
    public async Task Handle_WhenRoleWithGivenIdDoesNotExist_ShouldReturnNull()
    {
        var nonExistentId = Guid.NewGuid();
        var query = new GetRoleByIdQuery(nonExistentId);

        _roleRepositoryMock.GetByIdAsync(nonExistentId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromResult<RoleEntity?>(null));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();

        _mapperMock.DidNotReceive().Map<RoleDetailDto?>(Arg.Any<RoleEntity>());
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var roleId = Guid.NewGuid();
        var query = new GetRoleByIdQuery(roleId);
        var expectedException = new InvalidOperationException("Database error");

        _roleRepositoryMock.GetByIdAsync(roleId, Arg.Any<CancellationToken>())
                               .Returns(Task.FromException<RoleEntity?>(expectedException));

        Func<Task> act = async () => await _sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Database error");
    }
}
