using Libba.HubTo.Arcavis.Application.Features.Endpoint.CreateEndpoint;
using Libba.HubTo.Arcavis.Domain.Enums;
using FluentAssertions;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Endpoint.CreateEndpoint;

public class CreateEndpointCommandValidatorTests
{
    private readonly CreateEndpointCommandValidator _validator = new();

    [Fact]
    public void GivenValidCommand_WhenValidationIsPerformed_ThenShouldSucceed()
    {
        var command = new CreateEndpointCommand(
            ModuleName: "Invoices",
            ControllerName: "InvoicesController",
            ActionName: "GetInvoiceById",
            HttpVerb: HttpVerb.GET,
            Namespace: "Project.Features.Invoices"
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequiredFieldsTestData))]
    public void GivenInvalidRequiredField_WhenValidationIsPerformed_ThenShouldFailForThatField(string invalidValue, string expectedPropertyName)
    {
        var command = new CreateEndpointCommand(
            ModuleName: expectedPropertyName == nameof(CreateEndpointCommand.ModuleName) ? invalidValue : "ValidModule",
            ControllerName: expectedPropertyName == nameof(CreateEndpointCommand.ControllerName) ? invalidValue : "ValidController",
            ActionName: expectedPropertyName == nameof(CreateEndpointCommand.ActionName) ? invalidValue : "ValidAction",
            HttpVerb: HttpVerb.GET,
            Namespace: expectedPropertyName == nameof(CreateEndpointCommand.Namespace) ? invalidValue : "Valid.Namespace"
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
            nameof(CreateEndpointCommand.ModuleName),
            nameof(CreateEndpointCommand.ControllerName),
            nameof(CreateEndpointCommand.ActionName),
            nameof(CreateEndpointCommand.Namespace)
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
