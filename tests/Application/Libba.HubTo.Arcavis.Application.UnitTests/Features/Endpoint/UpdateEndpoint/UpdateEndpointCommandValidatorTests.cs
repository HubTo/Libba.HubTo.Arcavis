using Libba.HubTo.Arcavis.Application.Features.Endpoint.UpdateEndpoint;
using Libba.HubTo.Arcavis.Domain.Enums;
using FluentAssertions;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Endpoint.UpdateEndpoint;

public class UpdateEndpointCommandValidatorTests
{
    private readonly UpdateEndpointCommandValidator _validator = new();

    [Fact]
    public void GivenValidCommand_WhenValidationIsPerformed_ThenShouldSucceed()
    {
        var command = new UpdateEndpointCommand(
            Id: Guid.NewGuid(),
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
        var command = new UpdateEndpointCommand(
            Id: Guid.NewGuid(),
            ModuleName: expectedPropertyName == nameof(UpdateEndpointCommand.ModuleName) ? invalidValue : "ValidModule",
            ControllerName: expectedPropertyName == nameof(UpdateEndpointCommand.ControllerName) ? invalidValue : "ValidController",
            ActionName: expectedPropertyName == nameof(UpdateEndpointCommand.ActionName) ? invalidValue : "ValidAction",
            HttpVerb: HttpVerb.GET,
            Namespace: expectedPropertyName == nameof(UpdateEndpointCommand.Namespace) ? invalidValue : "Valid.Namespace"
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
            nameof(UpdateEndpointCommand.ModuleName),
            nameof(UpdateEndpointCommand.ControllerName),
            nameof(UpdateEndpointCommand.ActionName),
            nameof(UpdateEndpointCommand.Namespace)
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
        var command = new UpdateEndpointCommand(
           Id: Guid.Empty,
           ModuleName: "ValidModule",
           ControllerName: "ValidController",
           ActionName: "ValidAction",
           HttpVerb: HttpVerb.GET,
           Namespace: "Valid.Namespace"
       );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(UpdateEndpointCommand.Id));
    }
}
