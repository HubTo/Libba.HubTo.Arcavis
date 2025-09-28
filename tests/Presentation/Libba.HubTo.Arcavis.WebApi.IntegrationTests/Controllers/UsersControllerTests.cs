using FluentAssertions;
using Libba.HubTo.Arcavis.Application.Features.User.CreateUser;
using Libba.HubTo.Arcavis.Application.Features.User.GetAllUsers;
using Libba.HubTo.Arcavis.Application.Features.User.GetUserById;
using Libba.HubTo.Arcavis.Application.Features.User.UpdateUser;
using Libba.HubTo.Arcavis.WebApi.IntegrationTests.Dtos;
using System.Net;
using System.Net.Http.Json;

namespace Libba.HubTo.Arcavis.WebApi.IntegrationTests.Controllers;

public class UsersControllerTests : IClassFixture<ArcavisApiFactory>
{
    #region Dependencies
    private readonly HttpClient _client;

    public UsersControllerTests(ArcavisApiFactory factory)
    {
        _client = factory.CreateClient();
    }
    #endregion

    private async Task<Guid> CreateTestUserAsync(string phoneCode = "+90", string phoneNumber = null)
    {
        phoneNumber ??= $"555{DateTime.Now.Ticks.ToString().Substring(10)}";
        var command = new CreateUserCommand(
            PhoneCode: phoneCode,
            PhoneNumber: phoneNumber
        );

        var response = await _client.PostAsJsonAsync("/api/User", command);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>();
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
        var existingPhoneNumber = $"555{DateTime.Now.Ticks.ToString().Substring(10)}";

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
    }

    [Fact]
    public async Task CreateUser_WhenPhoneNumberAlreadyExists_ShouldReturnBadRequestWithValidationErrors()
    {
        var existingPhoneNumber = "5551112233";
        await CreateTestUserAsync(phoneCode: "+1", phoneNumber: existingPhoneNumber);

        var duplicateCommand = new CreateUserCommand(
            PhoneCode: "+1",
            PhoneNumber: existingPhoneNumber
        );

        var response = await _client.PostAsJsonAsync("/api/User", duplicateCommand);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();

        errorResponse.Should().NotBeNull();
        errorResponse.Title.Should().Be("One or more validation errors occurred.");
        errorResponse.Errors.Should().ContainKey(""); 
        errorResponse.Errors[""].Should().Contain("This phone number is already registered.");
    }

    [Fact]
    public async Task CreateUser_WhenSendingMalformedJson_ShouldReturnInternalServerErrorFromApiBehavior()
    {
        var malformedJsonPayload = new
        {
            PhoneCode = "+1",
            PhoneNumber = 5551234567
        };
        var content = JsonContent.Create(malformedJsonPayload);

        var response = await _client.PostAsync("/api/User", content);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var errorResponse = await response.Content.ReadFromJsonAsync<InternalErrorResponse>();

        errorResponse.Should().NotBeNull();
        errorResponse.Title.Should().Be("An internal server error has occurred.");
        errorResponse.Detail.Should().Be("Please try again later or contact support.");
    }

    [Fact]
    public async Task GetAll_WhenUsersExist_ShouldReturnOkAndListOfUsers()
    {
        // Arrange
        await CreateTestUserAsync(phoneNumber: $"555{DateTime.Now.Ticks.ToString().Substring(11)}1");
        await CreateTestUserAsync(phoneNumber: $"555{DateTime.Now.Ticks.ToString().Substring(11)}2");

        // Act
        var response = await _client.GetAsync("/api/User");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var users = await response.Content.ReadFromJsonAsync<List<UserListItemDto>>();
        users.Should().NotBeNull();
        users.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetById_WhenUserExists_ShouldReturnOkAndUser()
    {
        // Arrange
        var userId = await CreateTestUserAsync();

        // Act
        var response = await _client.GetAsync($"/api/User/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await response.Content.ReadFromJsonAsync<UserDetailDto>();
        user.Should().NotBeNull();
        user.Id.Should().Be(userId);
    }

    [Fact]
    public async Task GetById_WhenUserDoesNotExist_ShouldReturnOkWithNullUser()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/User/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateUser_WhenCommandIsValid_ShouldReturnOkAndReflectChanges()
    {
        // Arrange
        var userId = await CreateTestUserAsync();
        var updatedPhoneNumber = $"555{DateTime.Now.Ticks.ToString().Substring(10)}";
        var updateCommand = new UpdateUserCommand(
            Id: userId,
            PhoneCode: "+49",
            PhoneNumber: updatedPhoneNumber
        );

        // Act
        var response = await _client.PutAsJsonAsync("/api/User", updateCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returnedId = await response.Content.ReadFromJsonAsync<Guid>();
        returnedId.Should().Be(userId);

        // Verify that the user was actually updated
        var verifyResponse = await _client.GetAsync($"/api/User/{userId}");
        var updatedUser = await verifyResponse.Content.ReadFromJsonAsync<UserDetailDto>();
        updatedUser.PhoneNumber.Should().Be(updatedPhoneNumber);
        updatedUser.PhoneCode.Should().Be("+49");
    }

    [Fact]
    public async Task DeleteUser_WhenUserExists_ShouldReturnOkAndRemoveUser()
    {
        var userId = await CreateTestUserAsync();

        var response = await _client.DeleteAsync($"/api/User/{userId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var verifyResponse = await _client.GetAsync($"/api/User/{userId}");
        verifyResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteUser_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        var nonExistentId = Guid.NewGuid();

        var response = await _client.DeleteAsync($"/api/User/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}