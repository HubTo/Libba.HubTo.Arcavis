using Libba.HubTo.Arcavis.Application.Features.User.CreateUser;
using System.Net.Http.Json;
using FluentAssertions;
using System.Net;

namespace Libba.HubTo.Arcavis.WebApi.IntegrationTests.Controllers;

public class UsersControllerTests : IClassFixture<ArcavisApiFactory>
{
    private readonly HttpClient _client;

    public UsersControllerTests(ArcavisApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_WhenCommandIsValidAndPhoneNumberIsUnique_ShouldReturnCreatedAndUserId()
    {
        var uniquePhoneNumber = $"555{DateTime.Now.Ticks.ToString().Substring(10)}";
        var command = new CreateUserCommand(
            PhoneCode: "+90",
            PhoneNumber: uniquePhoneNumber
        );

        var response = await _client.PostAsJsonAsync("/api/User", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdUserId = await response.Content.ReadFromJsonAsync<Guid>();
        createdUserId.Should().NotBeEmpty();

    }

    [Fact]
    public async Task CreateUser_WhenPhoneNumberAlreadyExists_ShouldReturnBadRequest()
    {
        var existingPhoneNumber = "5551112233";
        var initialCommand = new CreateUserCommand(
            PhoneCode: "+1",
            PhoneNumber: existingPhoneNumber
        );
        var initialResponse = await _client.PostAsJsonAsync("/api/User", initialCommand);
        initialResponse.EnsureSuccessStatusCode();

        var duplicateCommand = new CreateUserCommand(
            PhoneCode: "+1",
            PhoneNumber: existingPhoneNumber
        );

        var response = await _client.PostAsJsonAsync("/api/User", duplicateCommand);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadAsStringAsync();
        errorResponse.Should().Contain("This phone number is already registered.");
    }
}