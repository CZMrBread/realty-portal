using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Features.Ruian.Entity;
using Server.Infrastructure.Database;
using Shared.RealtyAgency.CreateRealtyAgency;
using Shared.RealtyAgent;
using Shared.RealtyAgent.BecomeAgent;
using Shared.Ruian;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Shared.User;

namespace Server.Tests.Features.SRealty.Advert.CreateAdvert;

/// <summary>Runs the create routes against SQLite with a small hand-seeded RUIAN register.</summary>
public class CreateAdvertTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
    private const string AdvertRoute = "api/srealty/advert";

    // the seeded register: one street with one house in Praha
    private const int PragueCode = 554782;
    private const int VinohradyCode = 490156;
    private const int ItalskaCode = 458023;
    private const int HouseCode = 21730678;

    // --- access ---

    [Fact]
    public async Task CreateAdvert_WithoutAToken_IsRefused()
    {
        var response = await factory.CreateClient().PostAsJsonAsync(AdvertRoute, Flat("Praha"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateAdvert_ByAnAccountThatIsNotAnAgent_IsRefused()
    {
        var client = factory.CreateClient();
        var account = await TestAccounts.RegisterAsync(client, "plainuser");
        client.Authenticated(account.Token.AccessToken);

        var response = await client.PostAsJsonAsync(AdvertRoute, Flat("Praha") with { SellerId = account.Id });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CreateAdvert_ForAnotherSeller_IsRefused()
    {
        var (client, _) = await AgentClientAsync("wrongseller");

        var response = await client.PostAsJsonAsync(AdvertRoute, Flat("Praha") with { SellerId = Guid.NewGuid() });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Contains(AdvertErrors.SellerMismatch.Code, await response.Content.ReadAsStringAsync());
    }

    // --- the happy path ---

    [Fact]
    public async Task CreateAdvert_StoresTheAdvertAndReturnsIt()
    {
        var (client, userId) = await AgentClientAsync("creator");

        var created = await CreateAsync(client, Flat("Atlantis") with { SellerId = userId });

        Assert.Equal(CreateAdvertResponseStatusEnum.OK, created.Status);
        var advert = Assert.IsType<SrealityAdvertDto>(created.Advert);
        Assert.NotNull(advert.AdvertId);
        Assert.Equal(userId, advert.SellerId);
        Assert.Equal(AdvertTypeEnum.Flat, advert.AdvertType);

        var stored = await factory.CreateClient().GetFromJsonAsync<SrealityAdvertDto>($"{AdvertRoute}/{advert.AdvertId}");
        Assert.NotNull(stored);
        Assert.Equal(advert.Description, stored.Description);
        Assert.Equal(advert.AdvertPrice, stored.AdvertPrice);
    }

    [Fact]
    public async Task CreateAdvert_WithASubtypeOfAnotherCategory_IsRefused()
    {
        var (client, userId) = await AgentClientAsync("badsubtype");

        // a flat layout is not a subtype of a house: the rule lives in SrealityAdvertDto.Validate and runs only
        // once every field-level rule of a house holds
        var response = await client.PostAsJsonAsync(AdvertRoute, Flat("Praha") with
        {
            SellerId = userId,
            AdvertType = AdvertTypeEnum.House,
            AdvertSubtype = AdvertSubtypeEnum.TwoPlusKKApartments,
            AdvertRoomCount = AdvertRoomCountEnum.ThreeRooms,
            EstateArea = 400,
            ObjectType = ObjectTypeEnum.TwoStory,
            Basin = false
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("subtype", await response.Content.ReadAsStringAsync(), StringComparison.OrdinalIgnoreCase);
    }

    // --- the agency key route ---

    [Fact]
    public async Task CreateAdvert_WithAnAgencyKey_RefusesADuplicateKey()
    {
        var (client, userId) = await AgentClientAsync("rkagent");
        await FoundAgencyAsync(client, "rkagency");

        var first = await client.PostAsJsonAsync($"{AdvertRoute}/rk?advertRkId=RK-1", Flat("Praha") with { SellerId = userId });
        var second = await client.PostAsJsonAsync($"{AdvertRoute}/rk?advertRkId=RK-1", Flat("Praha") with { SellerId = userId });

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        Assert.Contains(AdvertErrors.RkIdTaken.Code, await second.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task CreateAdvert_WithAnAgencyKey_NeedsAnAgency()
    {
        var (client, userId) = await AgentClientAsync("loneagent");

        var response = await client.PostAsJsonAsync($"{AdvertRoute}/rk?advertRkId=RK-2", Flat("Praha") with { SellerId = userId });

        Assert.Equal(AgentErrors.NoAgency.StatusCode, response.StatusCode);
        Assert.Contains(AgentErrors.NoAgency.Code, await response.Content.ReadAsStringAsync());
    }

    // --- locality against the register ---

    [Fact]
    public async Task CreateAdvert_WithAnAddressPointCode_FillsTheAddressFromTheRegister()
    {
        await SeedRegisterAsync();
        var (client, userId) = await AgentClientAsync("addressagent");

        var created = await CreateAsync(client, Flat("praha") with
        {
            SellerId = userId,
            LocalityRuian = HouseCode,
            LocalityRuianLevel = RuianLevelEnum.Address
        });

        var advert = created.Advert!;
        Assert.Equal("Praha", advert.LocalityCity);
        Assert.Equal("Vinohrady", advert.LocalityCityPart);
        Assert.Equal("Italská", advert.LocalityStreet);
        Assert.Equal("12", advert.LocalityCp);
        Assert.Equal("5a", advert.LocalityCo);
        Assert.Equal(50.08, advert.LocalityLatitude);
        Assert.Equal(14.43, advert.LocalityLongitude);
        Assert.Equal(HouseCode, advert.LocalityRuian);
        Assert.Equal(RuianLevelEnum.Address, advert.LocalityRuianLevel);

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var stored = await db.SrealityAdverts.SingleAsync(a => a.Id == advert.AdvertId);
        Assert.Equal(HouseCode, stored.LocalityAddressPointCode);
        Assert.Equal(PragueCode, stored.LocalityMunicipalityCode);
    }

    [Fact]
    public async Task CreateAdvert_KeepsTheSendersCoordinates()
    {
        await SeedRegisterAsync();
        var (client, userId) = await AgentClientAsync("pinagent");

        var created = await CreateAsync(client, Flat("Praha") with
        {
            SellerId = userId,
            LocalityRuian = HouseCode,
            LocalityRuianLevel = RuianLevelEnum.Address,
            LocalityLatitude = 50.1,
            LocalityLongitude = 14.5
        });

        Assert.Equal(50.1, created.Advert!.LocalityLatitude);
        Assert.Equal(14.5, created.Advert.LocalityLongitude);
    }

    [Fact]
    public async Task CreateAdvert_WithACodeOfAnotherTown_IsRefused()
    {
        await SeedRegisterAsync();
        var (client, userId) = await AgentClientAsync("mismatchagent");

        var response = await client.PostAsJsonAsync(AdvertRoute, Flat("Brno") with
        {
            SellerId = userId,
            LocalityRuian = HouseCode,
            LocalityRuianLevel = RuianLevelEnum.Address
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("locality_city", await response.Content.ReadAsStringAsync());
    }

    [Theory]
    [InlineData(RuianLevelEnum.Address)]
    [InlineData(RuianLevelEnum.Street)]
    [InlineData(RuianLevelEnum.Municipality)]
    [InlineData(RuianLevelEnum.District)]
    public async Task CreateAdvert_WithAnUnknownCode_IsRefused(RuianLevelEnum level)
    {
        await SeedRegisterAsync();
        var (client, userId) = await AgentClientAsync($"unknown{level}");

        var response = await client.PostAsJsonAsync(AdvertRoute, Flat("Praha") with
        {
            SellerId = userId,
            LocalityRuian = 1,
            LocalityRuianLevel = level
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("locality_ruian", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task CreateAdvert_WithABuildingCode_IsRefusedAsUnsupported()
    {
        var (client, userId) = await AgentClientAsync("buildingagent");

        var response = await client.PostAsJsonAsync(AdvertRoute, Flat("Praha") with
        {
            SellerId = userId,
            LocalityRuian = 123,
            LocalityRuianLevel = RuianLevelEnum.Building
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("locality_ruian_level", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task CreateAdvert_WithoutACode_GeocodesTheNamesDownToTheHouse()
    {
        await SeedRegisterAsync();
        var (client, userId) = await AgentClientAsync("geocodeagent");

        var created = await CreateAsync(client, Flat("PRAHA") with
        {
            SellerId = userId,
            LocalityStreet = "italska",
            LocalityCp = "12"
        });

        Assert.Equal(HouseCode, created.Advert!.LocalityRuian);
        Assert.Equal(RuianLevelEnum.Address, created.Advert.LocalityRuianLevel);
        Assert.Equal("Italská", created.Advert.LocalityStreet);
        Assert.Equal(50.08, created.Advert.LocalityLatitude);
    }

    [Fact]
    public async Task CreateAdvert_WithOnlyAStreetName_ResolvesTheStreet()
    {
        await SeedRegisterAsync();
        var (client, userId) = await AgentClientAsync("streetagent");

        var created = await CreateAsync(client, Flat("Praha") with { SellerId = userId, LocalityStreet = "Italská" });

        Assert.Equal(ItalskaCode, created.Advert!.LocalityRuian);
        Assert.Equal(RuianLevelEnum.Street, created.Advert.LocalityRuianLevel);
    }

    [Fact]
    public async Task CreateAdvert_InAnUnknownTown_IsAcceptedUnplaced()
    {
        await SeedRegisterAsync();
        var (client, userId) = await AgentClientAsync("nowhereagent");

        var created = await CreateAsync(client, Flat("Atlantis") with { SellerId = userId });

        Assert.Equal("Atlantis", created.Advert!.LocalityCity);
        Assert.Null(created.Advert.LocalityRuian);
        Assert.Null(created.Advert.LocalityRuianLevel);
    }

    // --- validation attributes ---

    public static TheoryData<string, Func<SrealityAdvertDto, SrealityAdvertDto>> BrokenRules => new()
    {
        // [Required]
        { nameof(SrealityAdvertDto.AdvertFunction), a => a with { AdvertFunction = null } },
        { nameof(SrealityAdvertDto.AdvertPriceCurrency), a => a with { AdvertPriceCurrency = null } },
        { nameof(SrealityAdvertDto.LocalityCity), a => a with { LocalityCity = null } },
        { nameof(SrealityAdvertDto.Description), a => a with { Description = "" } },
        // [Range]
        { nameof(SrealityAdvertDto.AdvertPrice), a => a with { AdvertPrice = 0 } },
        { nameof(SrealityAdvertDto.UsableArea), a => a with { UsableArea = -1 } },
        { nameof(SrealityAdvertDto.LocalityLatitude), a => a with { LocalityLatitude = 91, LocalityLongitude = 14 } },
        // [EnumValue]
        { nameof(SrealityAdvertDto.AdvertType), a => a with { AdvertType = (AdvertTypeEnum)999 } },
        { nameof(SrealityAdvertDto.Furnished), a => a with { Furnished = (FurnishingEnum)999 } },
        // [RequiredIfValue]: a house needs its room count and estate area
        { nameof(SrealityAdvertDto.AdvertRoomCount), a => House(a) with { AdvertRoomCount = null } },
        { nameof(SrealityAdvertDto.EstateArea), a => House(a) with { EstateArea = null } },
        // IValidatableObject rules
        { nameof(SrealityAdvertDto.SellerRkId), a => a with { SellerRkId = "also-by-key" } },
        { nameof(SrealityAdvertDto.LocalityLongitude), a => a with { LocalityLatitude = 50 } },
        { nameof(SrealityAdvertDto.LocalityRuianLevel), a => a with { LocalityRuian = 554782 } },
        { nameof(SrealityAdvertDto.ApartmentNumber), a => House(a) with { ApartmentNumber = 5 } },
        { nameof(SrealityAdvertDto.Personal), a => a with { Ownership = OwnershipTypeEnum.Cooperative } }
    };

    [Theory]
    [MemberData(nameof(BrokenRules))]
    public async Task CreateAdvert_BreakingOneRule_IsRefusedOnThatField(string field,
        Func<SrealityAdvertDto, SrealityAdvertDto> breakRule)
    {
        var (client, userId) = await AgentClientAsync($"rule{field}");

        var response = await client.PostAsJsonAsync(AdvertRoute, breakRule(Flat("Praha") with { SellerId = userId }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(body.Contains(field) || body.Contains(JsonName(field)), $"{field} not named in: {body}");
    }

    [Fact]
    public async Task CreateAdvert_WithoutASeller_IsRefusedOnBothSellerFields()
    {
        var (client, _) = await AgentClientAsync("nosellerrule");

        var response = await client.PostAsJsonAsync(AdvertRoute, Flat("Praha"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(body.Contains(nameof(SrealityAdvertDto.SellerId)) || body.Contains(JsonName(nameof(SrealityAdvertDto.SellerId))), body);
    }

    /// <summary>Wire name of an advert field, which is what the validation problem may report.</summary>
    private static string JsonName(string property)
        => typeof(SrealityAdvertDto).GetProperty(property)!.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name;

    /// <summary>The flat turned into a valid house, for the rules that apply to houses only.</summary>
    private static SrealityAdvertDto House(SrealityAdvertDto flat) => flat with
    {
        AdvertType = AdvertTypeEnum.House,
        AdvertSubtype = AdvertSubtypeEnum.FamilyHouses,
        AdvertRoomCount = AdvertRoomCountEnum.ThreeRooms,
        EstateArea = 400,
        ObjectType = ObjectTypeEnum.TwoStory,
        Basin = false
    };

    // --- helpers ---

    private static async Task<CreateAdvertResponse> CreateAsync(HttpClient client, SrealityAdvertDto advert)
    {
        var response = await client.PostAsJsonAsync(AdvertRoute, advert);
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, body);
        return (await response.Content.ReadFromJsonAsync<CreateAdvertResponse>())!;
    }

    /// <summary>Registers an agent account and returns a client whose token carries the agent role.</summary>
    private async Task<(HttpClient Client, Guid UserId)> AgentClientAsync(string userName)
    {
        var client = factory.CreateClient();
        var account = await TestAccounts.RegisterAsync(client, userName);
        client.Authenticated(account.Token.AccessToken);

        var become = await client.PostAsJsonAsync("api/realty-agent/become", new BecomeAgentRequest
        {
            Name = $"Agent {userName}",
            RegistrationNumber = "12345678"
        });
        become.EnsureSuccessStatusCode();

        // the agent role travels in the token, so the one issued before becoming an agent has to be exchanged
        var refreshed = await client.PostAsJsonAsync("api/user/refresh",
            new RefreshTokenRequest { RefreshToken = account.Token.RefreshToken });
        refreshed.EnsureSuccessStatusCode();
        var tokens = (await refreshed.Content.ReadFromJsonAsync<TokenResponse>())!;

        return (factory.CreateClient().Authenticated(tokens.AccessToken), account.Id);
    }

    /// <summary>Founds an agency with the client's agent as its admin.</summary>
    private static async Task FoundAgencyAsync(HttpClient client, string name)
    {
        var response = await client.PostAsJsonAsync("api/realty-agency", new CreateRealtyAgencyRequest
        {
            Name = name,
            RegistrationNumber = Guid.NewGuid().ToString("N")[..8],
            Email = $"{name}@example.com"
        });
        response.EnsureSuccessStatusCode();
    }

    /// <summary>Puts one Praha street with one house into the otherwise empty register; idempotent.</summary>
    private async Task SeedRegisterAsync()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (await db.RuianAddressPoints.AnyAsync(a => a.Code == HouseCode))
        {
            return;
        }

        db.RuianRegions.Add(new RuianRegionEntity { Code = 19, Name = "Hlavní město Praha" });
        db.RuianDistricts.Add(new RuianDistrictEntity { Code = 3100, Name = "Praha", RegionCode = 19 });
        db.RuianMunicipalities.Add(new RuianMunicipalityEntity
            { Code = PragueCode, Name = "Praha", SearchName = "praha", DistrictCode = 3100 });
        db.RuianMunicipalityParts.Add(new RuianMunicipalityPartEntity
            { Code = VinohradyCode, Name = "Vinohrady", SearchName = "vinohrady", MunicipalityCode = PragueCode });
        db.RuianStreets.Add(new RuianStreetEntity
            { Code = ItalskaCode, Name = "Italská", SearchName = "italska", MunicipalityCode = PragueCode });
        db.RuianAddressPoints.Add(new RuianAddressPointEntity
        {
            Code = HouseCode,
            MunicipalityCode = PragueCode,
            MunicipalityPartCode = VinohradyCode,
            StreetCode = ItalskaCode,
            HouseNumberType = HouseNumberTypeEnum.Descriptive,
            HouseNumber = 12,
            OrientationNumber = 5,
            OrientationLetter = "a",
            PostalCode = "12000",
            Latitude = 50.08,
            Longitude = 14.43
        });
        await db.SaveChangesAsync();
    }

    /// <summary>A valid flat for sale in the given town; the seller is filled in by the test.</summary>
    private static SrealityAdvertDto Flat(string city) => new()
    {
        AdvertFunction = AdvertFunctionEnum.Sell,
        AdvertLifetime = AdvertLifetimeEnum.ThirtyDays,
        AdvertType = AdvertTypeEnum.Flat,
        AdvertSubtype = AdvertSubtypeEnum.TwoPlusKKApartments,
        AdvertPrice = 5_000_000,
        AdvertPriceCurrency = AdvertPriceCurrencyEnum.CZK,
        AdvertPriceUnit = AdvertPriceUnitEnum.PerRealty,
        LocalityCity = city,
        LocalityInaccuracyLevel = 0,
        Description = $"Flat in {city}",
        UsableArea = 60,
        BuildingCondition = BuildingConditionEnum.Good,
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
}
