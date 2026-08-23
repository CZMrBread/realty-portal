using System.Net;
using System.Net.Http.Json;
using Shared.User;
using Shared.User.Register;

namespace Server.Tests.Features.User.Register;

public class RegisterTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RegisterTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsTokensAndAccount()
    {
        var request = new RegisterUserRequest
        {
            UserName = "newuser",
            Email = "newuser@example.com",
            Password = "Password1",
            ConfirmPassword = "Password1"
        };

        var response = await _client.PostAsJsonAsync("api/user/register", request);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RegisterUserResponse>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.Token.AccessToken);
        Assert.NotEmpty(result.Token.RefreshToken);
        Assert.Equal(request.UserName, result.UserName);
        // a freshly registered account holds no portal role yet
        Assert.Empty(result.Roles);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
    {
        var request = new RegisterUserRequest
        {
            UserName = "duplicate1",
            Email = "duplicate@example.com",
            Password = "Password1",
            ConfirmPassword = "Password1"
        };
        (await _client.PostAsJsonAsync("api/user/register", request)).EnsureSuccessStatusCode();

        var second = await _client.PostAsJsonAsync("api/user/register", request with { UserName = "duplicate2" });

        Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);
    }
}
