using Libba.HubTo.Arcavis.Application.Features.User.CreateUser;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.User;
using Libba.HubTo.Arcavis.Domain.Entities;
using FluentValidation.TestHelper;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.User.CreateUser;

public class CreateUserCommandValidatorTests
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly CreateUserCommandValidator _sut; // System Under Test

    public CreateUserCommandValidatorTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _sut = new CreateUserCommandValidator(_userRepositoryMock);
    }

    [Fact]
    public async Task GivenValidCommand_AndUniquePhoneNumber_WhenValidationIsPerformed_ThenShouldSucceed()
    {
        var command = new CreateUserCommand(
            PhoneCode: "+90",
            PhoneNumber: "5551234567"
        );

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _sut.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task GivenExistingPhoneNumber_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateUserCommand(
            PhoneCode: "+1",
            PhoneNumber: "8005551234"
        );

        _userRepositoryMock.ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await _sut.TestValidateAsync(command);

        result.Errors.Should().ContainSingle(e =>
            string.IsNullOrEmpty(e.PropertyName) &&
            e.ErrorMessage == "This phone number is already registered."
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GivenEmptyPhoneNumber_WhenValidationIsPerformed_ThenShouldFailForPhoneNumber(string phoneNumber)
    {
        var command = new CreateUserCommand(PhoneCode: "+1", PhoneNumber: phoneNumber);

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
              .WithErrorMessage("Phone number cannot be empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GivenEmptyPhoneCode_WhenValidationIsPerformed_ThenShouldFailForPhoneCode(string phoneCode)
    {
        var command = new CreateUserCommand(PhoneCode: phoneCode, PhoneNumber: "5551234567");

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.PhoneCode)
              .WithErrorMessage("Phone code cannot be empty.");
    }

    [Fact]
    public async Task GivenPhoneNumber_ThatIsTooShort_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateUserCommand(PhoneCode: "+1", PhoneNumber: "123456");

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
              .WithErrorMessage("Phone number must be between 7 and 20 digits.");
    }

    [Fact]
    public async Task GivenPhoneNumber_ThatIsTooLong_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateUserCommand(PhoneCode: "+1", PhoneNumber: "123456789098765432100");

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
              .WithErrorMessage("Phone number must be between 7 and 20 digits.");
    }

    [Fact]
    public async Task GivenEmptyPhoneNumber_WhenValidationIsPerformed_ThenRepositoryShouldNotBeCalled()
    {
        var command = new CreateUserCommand(PhoneCode: "+1", PhoneNumber: "");

        await _sut.TestValidateAsync(command);

        await _userRepositoryMock.DidNotReceive().ExistsAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GivenPhoneCode_ThatIsTooShort_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateUserCommand(PhoneCode: "1", PhoneNumber: "1234567");

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.PhoneCode)
              .WithErrorMessage("Phone code must be between 2 and 10 digits.");
    }

    [Fact]
    public async Task GivenPhoneCode_ThatIsTooLong_WhenValidationIsPerformed_ThenShouldFail()
    {
        var command = new CreateUserCommand(PhoneCode: "12345678910", PhoneNumber: "1234567");

        var result = await _sut.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.PhoneCode)
              .WithErrorMessage("Phone code must be between 2 and 10 digits.");
    }
}
