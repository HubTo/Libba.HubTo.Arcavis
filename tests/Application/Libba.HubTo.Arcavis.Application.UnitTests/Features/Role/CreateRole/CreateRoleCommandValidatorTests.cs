using Libba.HubTo.Arcavis.Application.Features.Role.CreateRole;
using FluentAssertions;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Role.CreateRole;

public class CreateRoleCommandValidatorTests
{
    private readonly CreateRoleCommandValidator _validator = new();

    [Fact]
    public void GivenValidCommand_WhenValidationIsPerformed_ThenShouldSucceed()
    {
        var command = new CreateRoleCommand(
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
        var command = new CreateRoleCommand(
            Name: expectedPropertyName == nameof(CreateRoleCommand.Name) ? invalidValue : "ValidName",
            Description: expectedPropertyName == nameof(CreateRoleCommand.Description) ? invalidValue : "ValidDescription"
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
            nameof(CreateRoleCommand.Name),
            nameof(CreateRoleCommand.Description)
        };

        foreach (var prop in properties)
        {
            foreach (var invalidStr in invalidStrings)
            {
                yield return new object[] { invalidStr, prop };
            }
        }
    }
}
