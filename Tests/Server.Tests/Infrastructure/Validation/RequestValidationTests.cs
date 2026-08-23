using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Shared.RealtyAgent.BecomeAgent;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Shared.User;
using Shared.User.Register;

namespace Server.Tests.Infrastructure.Validation;

/// <summary>
/// Pins the behaviour the built-in request validation is relied on for. The custom ValidationFilter was
/// removed in favour of it, so these tests are what proves the replacement covers the same ground: every
/// complex argument is checked, and a type that validates across its own fields still gets its say.
/// </summary>
public class RequestValidationTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
    /// <summary>
    /// A plain attribute on a request that does not implement IValidatableObject has to be enforced. The
    /// removed filter only ever looked at IValidatableObject arguments, so this went unchecked.
    /// </summary>
    [Fact]
    public async Task Register_with_mismatched_confirmation_is_refused()
    {
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("api/user/register", new RegisterUserRequest
        {
            UserName = "mismatch-user",
            Email = "mismatch-user@example.com",
            Password = "Password1",
            ConfirmPassword = "Password2"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(nameof(RegisterUserRequest.ConfirmPassword), await response.Content.ReadAsStringAsync());
    }

    /// <summary>A request whose fields are individually fine but which breaks a rule spanning two of them has to be refused.</summary>
    [Fact]
    public async Task Advert_breaking_a_cross_field_rule_is_refused()
    {
        var (client, _) = await AgentClientAsync("crossfield-agent");

        // Flat is not a valid subtype of House: the rule lives in SrealityAdvertDto.Validate, not on any one field
        var response = await client.PostAsJsonAsync("api/srealty/advert", ValidHouse() with
        {
            // Flat is not a valid subtype of House
            AdvertSubtype = AdvertSubtypeEnum.TwoPlusKKApartments
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(nameof(SrealityAdvertDto.AdvertSubtype), await response.Content.ReadAsStringAsync());
    }

    /// <summary>An advert that breaks no rule at all has to reach the handler and be stored.</summary>
    [Fact]
    public async Task Valid_advert_is_accepted()
    {
        var (client, userId) = await AgentClientAsync("valid-advert-agent");

        var response = await client.PostAsJsonAsync("api/srealty/advert", ValidHouse() with
        {
            SellerRkId = null,
            SellerId = userId
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();
        Assert.NotNull(created);
    }

    /// <summary>An advert every field-level rule is happy with, so that a test can break exactly one rule of its own.</summary>
    private static SrealityAdvertDto ValidHouse() => new()
    {
        SellerRkId = "seller-1",
        AdvertFunction = AdvertFunctionEnum.Sell,
        AdvertLifetime = AdvertLifetimeEnum.ThirtyDays,
        AdvertType = AdvertTypeEnum.House,
        AdvertSubtype = AdvertSubtypeEnum.FamilyHouses,
        AdvertRoomCount = AdvertRoomCountEnum.ThreeRooms,
        Cellar = true,
        Basin = false,
        Garage = true,
        ParkingLots = true,
        UsableArea = 120,
        EstateArea = 400,
        BuildingCondition = BuildingConditionEnum.Good,
        BuildingType = BuildingTypeEnum.Brick,
        ObjectType = ObjectTypeEnum.TwoStory,
        LocalityCity = "Praha",
        LocalityInaccuracyLevel = 1,
        Description = "A house with a garden.",
        AdvertPrice = 9_500_000,
        AdvertPriceCurrency = AdvertPriceCurrencyEnum.CZK,
        AdvertPriceUnit = AdvertPriceUnitEnum.PerRealty
    };

    /// <summary>Registers an account, gives it an agent profile, and returns a client holding a token that carries the agent role.</summary>
    private async Task<(HttpClient Client, Guid UserId)> AgentClientAsync(string userName)
    {
        var client = factory.CreateClient();
        var account = await TestAccounts.RegisterAsync(client, userName);

        var become = await client.Authenticated(account.Token.AccessToken)
            .PostAsync("api/realtyagent/become", null);
        become.EnsureSuccessStatusCode();
        Assert.NotNull(await become.Content.ReadFromJsonAsync<BecomeAgentResponse>());

        // the agent role travels as a claim, so the token has to be renewed before the agent routes admit it
        var refreshed = await client.PostAsJsonAsync("api/user/refresh",
            new RefreshTokenRequest { RefreshToken = account.Token.RefreshToken });
        refreshed.EnsureSuccessStatusCode();
        var tokens = (await refreshed.Content.ReadFromJsonAsync<TokenResponse>())!;

        return (factory.CreateClient().Authenticated(tokens.AccessToken), account.Id);
    }
}
