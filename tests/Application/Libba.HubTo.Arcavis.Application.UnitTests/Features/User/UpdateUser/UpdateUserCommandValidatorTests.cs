using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Application.Features.User.UpdateUser;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentValidation.TestHelper;
using System.Linq.Expressions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.User.UpdateUser;

public class UpdateUserCommandValidatorTests
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly UpdateUserCommandValidator _sut;

    public UpdateUserCommandValidatorTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _sut = new UpdateUserCommandValidator(_userRepositoryMock);
    }

    [Fact]
    public async Task GivenValidCommand_WhenPhoneNumberIsNotChanged_ThenShouldSucceed()
    {
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand(userId, "+90", "5551234567");

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _sut.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task GivenValidCommand_AndUniqueNewPhoneNumber_WhenValidationIsPerformed_ThenShouldSucceed()
    {
        var userId = Guid.NewGuid();
        var command = new UpdateUserCommand(userId, "+90", "5551234567");

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _sut.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GivenEmptyPhoneNumber_WhenValidationIsPerformed_ThenShouldFailForPhoneNumber(string phoneNumber)
    {
        var command = new UpdateUserCommand(Guid.NewGuid(), "+1", phoneNumber); // << Değişti

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
              .WithErrorMessage("Phone number cannot be empty.");
    }

    [Fact]
    public async Task GivenEmptyId_WhenValidationIsPerformed_ThenShouldFailForId()
    {
        var command = new UpdateUserCommand(Guid.Empty, "+1", "1234567");

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}

