using Libba.HubTo.Arcavis.Application.Features.UserRole.UpdateUserRole;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentValidation.TestHelper;
using System.Linq.Expressions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.UserRole.UpdateUserRole;

public class UpdateUserRoleCommandValidatorTests
{
    #region Dependencies
    private readonly IUserRepository _userRepositoryMock;
    private readonly IRoleRepository _roleRepositoryMock;
    private readonly UpdateUserRoleCommandValidator _sut;

    public UpdateUserRoleCommandValidatorTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _roleRepositoryMock = Substitute.For<IRoleRepository>();
        _sut = new UpdateUserRoleCommandValidator(_userRepositoryMock, _roleRepositoryMock);
    }
    #endregion

    [Fact]
    public async Task GivenValidCommand_AndExistingUserAndRole_WhenValidationIsPerformed_ThenShouldSucceed()
    {
        var command = new UpdateUserRoleCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(true);
        _roleRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<RoleEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _sut.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task GivenEmptyId_WhenValidationIsPerformed_ThenShouldFailForId()
    {
        var command = new UpdateUserRoleCommand(Id: Guid.Empty, UserId: Guid.NewGuid(), RoleId: Guid.NewGuid());
        var result = await _sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public async Task GivenEmptyUserId_WhenValidationIsPerformed_ThenShouldFailForUserId()
    {
        var command = new UpdateUserRoleCommand(Id: Guid.NewGuid(), UserId: Guid.Empty, RoleId: Guid.NewGuid());
        var result = await _sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task GivenEmptyRoleId_WhenValidationIsPerformed_ThenShouldFailForRoleId()
    {
        var command = new UpdateUserRoleCommand(Id: Guid.NewGuid(), UserId: Guid.NewGuid(), RoleId: Guid.Empty);
        var result = await _sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.RoleId);
    }

    [Fact]
    public async Task GivenNonExistentUser_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new UpdateUserRoleCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(false);
        _roleRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<RoleEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId)
              .WithErrorMessage("The specified User does not exist.");
    }

    [Fact]
    public async Task GivenNonExistentRole_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new UpdateUserRoleCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(true);
        _roleRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<RoleEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(false);

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.RoleId)
              .WithErrorMessage("The specified Role does not exist.");
    }
}
