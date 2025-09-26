using Libba.HubTo.Arcavis.Application.Features.Role.UpdateRole;
using Libba.HubTo.Arcavis.Domain.Enums;
using FluentAssertions;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Role.UpdateRole;

public class UpdateRoleCommandValidatorTests
{
    private readonly UpdateRoleCommandValidator _validator = new();

    [Fact]
    public void GivenValidCommand_WhenValidationIsPerformed_ThenShouldSucceed()
    {
        var command = new UpdateRoleCommand(
            Id: Guid.NewGuid(),
            Name: "Invoices",
            Description: "InvoicesDescription"
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequiredFieldsTestData))]
    public void GivenInvalidRequiredField_WhenValidationIsPerformed_ThenShouldFailForThatField(string invalidValue, string expectedPropertyName)
    {
        var command = new UpdateRoleCommand(
            Id: Guid.NewGuid(),
            Name: expectedPropertyName == nameof(UpdateRoleCommand.Name) ? invalidValue : "ValidName",
            Description: expectedPropertyName == nameof(UpdateRoleCommand.Description) ? invalidValue : "ValidDescription"
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == expectedPropertyName);
    }

    public static IEnumerable<object[]> GetInvalidRequiredFieldsTestData()
    {
        string[] invalidStrings = { null, "", "   " };
        string[] properties =
        {
            nameof(UpdateRoleCommand.Name),
            nameof(UpdateRoleCommand.Description),
        };

        foreach (var prop in properties)
        {
            foreach (var invalidStr in invalidStrings)
            {
                yield return new object[] { invalidStr, prop };
            }
        }
    }

    [Fact]
    public void GivenEmptyId_WhenValidationIsPerformed_ThenShouldFailForId()
    {
        var command = new UpdateRoleCommand(
            Id: Guid.Empty,
            Name: "Invoices",
            Description: "InvoicesDescription"
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(UpdateRoleCommand.Id));
    }
}
