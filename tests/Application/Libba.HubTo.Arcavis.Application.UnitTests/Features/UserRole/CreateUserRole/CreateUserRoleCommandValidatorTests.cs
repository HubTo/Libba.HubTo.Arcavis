using Libba.HubTo.Arcavis.Application.Features.UserRole.CreateUserRole;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.UserRole;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Role;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentValidation.TestHelper;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.UserRole.CreateUserRole;

public class CreateUserRoleCommandValidatorTests
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly IRoleRepository _roleRepositoryMock;
    private readonly IUserRoleRepository _userRoleRepositoryMock;
    private readonly CreateUserRoleCommandValidator _sut;

    public CreateUserRoleCommandValidatorTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _roleRepositoryMock = Substitute.For<IRoleRepository>();
        _userRoleRepositoryMock = Substitute.For<IUserRoleRepository>();

        _sut = new CreateUserRoleCommandValidator(_userRepositoryMock, _roleRepositoryMock, _userRoleRepositoryMock);
    }

    [Fact]
    public async Task GivenValidCommandAndDependencies_WhenValidationIsPerformed_ThenShouldSucceed()
    {
        // Arrange
        var command = new CreateUserRoleCommand(Guid.NewGuid(), Guid.NewGuid());

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(true);
        _roleRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<RoleEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(true);
        _userRoleRepositoryMock.DoesRelationExistAsync(command.UserId, command.RoleId, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _sut.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task GivenEmptyUserId_WhenValidationIsPerformed_ThenShouldFailForUserId()
    {
        var command = new CreateUserRoleCommand(UserId: Guid.Empty, RoleId: Guid.NewGuid());
        var result = await _sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task GivenEmptyRoleId_WhenValidationIsPerformed_ThenShouldFailForRoleId()
    {
        var command = new CreateUserRoleCommand(UserId: Guid.NewGuid(), RoleId: Guid.Empty);
        var result = await _sut.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.RoleId);
    }

    [Fact]
    public async Task GivenNonExistentUser_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateUserRoleCommand(Guid.NewGuid(), Guid.NewGuid());

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(false);
        _roleRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<RoleEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId)
              .WithErrorMessage("The specified User does not exist.");
    }

    [Fact]
    public async Task GivenNonExistentRole_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateUserRoleCommand(Guid.NewGuid(), Guid.NewGuid());

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(true);
        _roleRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<RoleEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(false);

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.RoleId)
              .WithErrorMessage("The specified Role does not exist.");
    }

    [Fact]
    public async Task GivenExistingRelation_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateUserRoleCommand(Guid.NewGuid(), Guid.NewGuid());

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(true);
        _roleRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<RoleEntity, bool>>>(), Arg.Any<CancellationToken>()).Returns(true);
        _userRoleRepositoryMock.DoesRelationExistAsync(command.UserId, command.RoleId, Arg.Any<CancellationToken>()).Returns(true); // İlişki ZATEN VAR

        var result = await _sut.TestValidateAsync(command);

        result.Errors.Should().ContainSingle(e =>
            string.IsNullOrEmpty(e.PropertyName) &&
            e.ErrorMessage.Contains("already exists")
        );
    }
}

