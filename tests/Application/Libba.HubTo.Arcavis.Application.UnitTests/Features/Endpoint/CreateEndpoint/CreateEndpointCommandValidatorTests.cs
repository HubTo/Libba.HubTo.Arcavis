using FluentAssertions;
using Libba.HubTo.Arcavis.Application.Features.Endpoint.CreateEndpoint;
using Libba.HubTo.Arcavis.Application.Interfaces.Repositories.Endpoint;
using Libba.HubTo.Arcavis.Domain.Enums;
using NSubstitute;

namespace Libba.HubTo.Arcavis.Application.UnitTests.Features.Endpoint.CreateEndpoint;

public class CreateEndpointCommandValidatorTests
{
    private readonly CreateEndpointCommandValidator _validator = new CreateEndpointCommandValidator();

    [Fact]
    public void WhenCommandIsValid_ShouldNotHaveValidationError()
    {
        var command = new CreateEndpointCommand("Invoices", "InvoicesController", "GetInvoiceById", HttpVerb.GET, "Project.Features.Invoices");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void WhenModuleNameIsInvalid_ShouldHaveValidationError(string moduleName)
    {
        var command = new CreateEndpointCommand(moduleName, "Controller", "Action", HttpVerb.GET, "Namespace");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ModuleName");
    }

    [Fact]
    public void WhenControllerNameIsEmpty_ShouldHaveValidationError()
    {
        var command = new CreateEndpointCommand("Module", "", "Action", HttpVerb.GET, "Namespace");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(null, "ControllerName")]
    [InlineData("", "ControllerName")]
    [InlineData(" ", "ControllerName")]
    [InlineData(null, "ActionName")]
    [InlineData("", "ActionName")]
    [InlineData(" ", "ActionName")]
    [InlineData(null, "ModuleName")]
    [InlineData("", "ModuleName")]
    [InlineData(" ", "ModuleName")]
    [InlineData(null, "Namespace")]
    [InlineData("", "Namespace")]
    [InlineData(" ", "Namespace")]
    public void WhenRequiredFieldsAreInvalid_ShouldHaveValidationError(string value, string propertyName)
    {
        var command = new CreateEndpointCommand(
            propertyName == "ModuleName" ? value : "ValidModule",
            propertyName == "ControllerName" ? value : "ValidController",
            propertyName == "ActionName" ? value : "ValidAction",
            HttpVerb.GET,
            propertyName == "Namespace" ? value : "Valid.Namespace"
        );

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == propertyName);
    }
}
