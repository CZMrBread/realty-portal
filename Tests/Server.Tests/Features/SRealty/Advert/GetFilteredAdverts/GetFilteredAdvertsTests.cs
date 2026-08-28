using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Shared.RealtyAgent.BecomeAgent;
using Shared.Shared;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.Enums;
using Shared.SRealty.Advert.ListAdverts;
using Shared.User;

namespace Server.Tests.Features.SRealty.Advert.GetFilteredAdverts;

/// <summary>
/// Runs the public listing against SQLite. Everything but the full-text search is exercised here; that one is
/// PostgreSQL-only and has no counterpart on the test database.
/// </summary>
public class GetFilteredAdvertsTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
    [Fact]
    public async Task GetFilteredAdverts_WithoutCriteria_ListsEveryAdvertNewestFirst()
    {
        var client = await AgentClientAsync("listingagent");
        var older = await CreateAdvertAsync(client, Flat("Praha", 5_000_000, 60));
        var newer = await CreateAdvertAsync(client, Flat("Brno", 4_000_000, 50));

        var page = await ReadAsync(factory.CreateClient(), "api/srealty/advert");

        Assert.True(page.TotalCount >= 2);
        var ids = page.Items.Select(Id).ToList();
        Assert.True(ids.IndexOf(newer) < ids.IndexOf(older));
    }

    [Fact]
    public async Task GetFilteredAdverts_NarrowsByLayoutAreaPriceAndCondition()
    {
        var client = await AgentClientAsync("filteragent");
        var match = await CreateAdvertAsync(client,
            Flat("Olomouc", 3_000_000, 55, AdvertSubtypeEnum.TwoPlusKKApartments, BuildingConditionEnum.Good));
        await CreateAdvertAsync(client,
            Flat("Olomouc", 3_000_000, 55, AdvertSubtypeEnum.ThreePlusOneApartments, BuildingConditionEnum.Good));
        await CreateAdvertAsync(client,
            Flat("Olomouc", 3_000_000, 90, AdvertSubtypeEnum.TwoPlusKKApartments, BuildingConditionEnum.Good));
        await CreateAdvertAsync(client,
            Flat("Olomouc", 9_000_000, 55, AdvertSubtypeEnum.TwoPlusKKApartments, BuildingConditionEnum.Good));
        await CreateAdvertAsync(client,
            Flat("Olomouc", 3_000_000, 55, AdvertSubtypeEnum.TwoPlusKKApartments, BuildingConditionEnum.Poor));

        var page = await ReadAsync(factory.CreateClient(),
            "api/srealty/advert?localityCity=olom"
            + $"&advertSubtypes={AdvertSubtypeEnum.TwoPlusKKApartments}&advertSubtypes={AdvertSubtypeEnum.OnePlusKKApartments}"
            + "&areaFrom=50&areaTo=60&priceFrom=1000000&priceTo=5000000"
            + $"&buildingConditions={BuildingConditionEnum.Good}&buildingConditions={BuildingConditionEnum.VeryGood}");

        Assert.Equal(1, page.TotalCount);
        Assert.Equal(match, Id(page.Items.Single()));
    }

    [Fact]
    public async Task GetFilteredAdverts_UsesTheEstateAreaForLand()
    {
        var client = await AgentClientAsync("landagent");
        var land = await CreateAdvertAsync(client, Land("Tábor", 1_000_000, 2_000));
        await CreateAdvertAsync(client, Flat("Tábor", 1_000_000, 2_000));

        var page = await ReadAsync(factory.CreateClient(),
            $"api/srealty/advert?localityCity=T%C3%A1bor&areaFrom=1500&advertType={AdvertTypeEnum.Land}");

        Assert.Equal(land, Id(page.Items.Single()));
    }

    [Fact]
    public async Task GetFilteredAdverts_SortsByPriceAndByArea()
    {
        var client = await AgentClientAsync("sortagent");
        var cheapSmall = await CreateAdvertAsync(client, Flat("Zlín", 1_000_000, 80));
        var dearLarge = await CreateAdvertAsync(client, Flat("Zlín", 2_000_000, 120));

        var byPrice = await ReadAsync(factory.CreateClient(),
            $"api/srealty/advert?localityCity=Zl%C3%ADn&sort={AdvertSortEnum.PriceDescending}");
        var byArea = await ReadAsync(factory.CreateClient(),
            $"api/srealty/advert?localityCity=Zl%C3%ADn&sort={AdvertSortEnum.AreaAscending}");

        Assert.Equal([dearLarge, cheapSmall], byPrice.Items.Select(Id));
        Assert.Equal([cheapSmall, dearLarge], byArea.Items.Select(Id));
    }

    [Fact]
    public async Task GetFilteredAdverts_PagesTheResult()
    {
        var client = await AgentClientAsync("pagingagent");
        for (var i = 0; i < 3; i++)
        {
            await CreateAdvertAsync(client, Flat("Opava", 1_000_000 + i, 40));
        }

        var page = await ReadAsync(factory.CreateClient(), "api/srealty/advert?localityCity=Opava&page=2&pageSize=2");

        Assert.Equal(3, page.TotalCount);
        Assert.Equal(2, page.TotalPages);
        Assert.Equal(2, page.Page);
        Assert.Single(page.Items);
    }

    [Theory]
    [InlineData("page=0")]
    [InlineData("pageSize=0")]
    [InlineData("pageSize=101")]
    [InlineData("sort=99")]
    public async Task GetFilteredAdverts_RefusesAnInvalidPageOrOrder(string query)
    {
        var response = await factory.CreateClient().GetAsync("api/srealty/advert?" + query);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<HttpClient> AgentClientAsync(string userName)
    {
        var client = factory.CreateClient();
        var account = await TestAccounts.RegisterAsync(client, userName);
        client.Authenticated(account.Token.AccessToken);
        var become = await client.PostAsync("api/realtyagent/become", null);
        become.EnsureSuccessStatusCode();
        var agent = (await become.Content.ReadFromJsonAsync<BecomeAgentResponse>())!;

        // the agent role travels in the token, so the one issued before becoming an agent has to be exchanged
        var refreshed = await client.PostAsJsonAsync("api/user/refresh",
            new RefreshTokenRequest { RefreshToken = account.Token.RefreshToken });
        refreshed.EnsureSuccessStatusCode();
        var tokens = (await refreshed.Content.ReadFromJsonAsync<TokenResponse>())!;
        client.Authenticated(tokens.AccessToken);
        client.DefaultRequestHeaders.Add(SellerHeader, agent.UserId.ToString());
        return client;
    }

    /// <summary>Carries the seller identifier from <see cref="AgentClientAsync"/> to <see cref="CreateAdvertAsync"/> without a second type for it.</summary>
    private const string SellerHeader = "X-Test-Seller";

    private static async Task<Guid> CreateAdvertAsync(HttpClient client, SrealityAdvertDto advert)
    {
        advert.SellerId = Guid.Parse(client.DefaultRequestHeaders.GetValues(SellerHeader).Single());
        var response = await client.PostAsJsonAsync("api/srealty/advert", advert);
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, body);
        var created = await response.Content.ReadFromJsonAsync<JsonElement>();
        return Id(created.GetProperty("advert"));
    }

    private static async Task<PagedResult<JsonElement>> ReadAsync(HttpClient client, string url)
    {
        var response = await client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, body);
        return (await response.Content.ReadFromJsonAsync<PagedResult<JsonElement>>())!;
    }

    /// <summary>Identifier of an advert read off the raw JSON: SrealityAdvertDto.AdvertId is marked JsonIgnore(WhenReading), so a deserialized DTO never carries it.</summary>
    private static Guid Id(JsonElement advert) => advert.GetProperty("advert_id").GetGuid();

    private static SrealityAdvertDto Flat(string city, double price, int usableArea,
        AdvertSubtypeEnum subtype = AdvertSubtypeEnum.TwoPlusKKApartments,
        BuildingConditionEnum condition = BuildingConditionEnum.Good)
        => new()
        {
            AdvertFunction = AdvertFunctionEnum.Sell,
            AdvertLifetime = AdvertLifetimeEnum.ThirtyDays,
            AdvertType = AdvertTypeEnum.Flat,
            AdvertSubtype = subtype,
            AdvertPrice = price,
            AdvertPriceCurrency = AdvertPriceCurrencyEnum.CZK,
            AdvertPriceUnit = AdvertPriceUnitEnum.PerRealty,
            LocalityCity = city,
            LocalityInaccuracyLevel = 0,
            Description = $"Flat in {city}",
            UsableArea = usableArea,
            BuildingCondition = condition,
            BuildingType = BuildingTypeEnum.Brick,
            FloorNumber = 1,
            Balcony = false,
            Loggia = false,
            Terrace = false,
            Cellar = false,
            Garage = false,
            ParkingLots = false,
            Ownership = OwnershipTypeEnum.Personal
        };

    private static SrealityAdvertDto Land(string city, double price, int estateArea)
        => new()
        {
            AdvertFunction = AdvertFunctionEnum.Sell,
            AdvertLifetime = AdvertLifetimeEnum.ThirtyDays,
            AdvertType = AdvertTypeEnum.Land,
            AdvertSubtype = AdvertSubtypeEnum.ResidentialLand,
            AdvertPrice = price,
            AdvertPriceCurrency = AdvertPriceCurrencyEnum.CZK,
            AdvertPriceUnit = AdvertPriceUnitEnum.PerRealty,
            LocalityCity = city,
            LocalityInaccuracyLevel = 0,
            Description = $"Land in {city}",
            EstateArea = estateArea
        };
}
